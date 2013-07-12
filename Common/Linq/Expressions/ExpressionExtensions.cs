using System.Web.Mvc;
using Avantech.Common.ComponentModel.DataAnnotations;
using Avantech.Common.Reflection;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Avantech.Common.Linq.Expressions
{
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Gets the member name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TMember">Type of the object's member.</typeparam>
        /// <param name="accessor">An expression that evaluates to an objects member.</param>
        /// <returns>The string name of the member.</returns>
        public static string MemberName<T, TMember>(this Expression<Func<T, TMember>> accessor)
    	{
    		var memberExpression = accessor.Body as MemberExpression;
    		if (memberExpression == null)
    			throw new InvalidOperationException("Expression must be a member expression");

    		return memberExpression.Member.Name;
    	}

        /// <summary>
        /// Determines whether the member specified in the expression has the specified attribute />.
        /// </summary>
        /// <typeparam name="T">The Type of the object.</typeparam>
        /// <typeparam name="TMember">The type of the object's member.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="attributeType">The attribute type to check for.</param>
        /// <param name="inherited">Whether base classes should also be checked for the attributed.</param>
        /// <returns>
        ///   <c>true</c> if the specified expression is required; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasAttribute<T, TMember>(
            this Expression<Func<T, TMember>> expression, 
            Type attributeType, 
            bool inherited = false
        ) {
    		var memberExpression = expression.Body as MemberExpression;
    		
            if(!attributeType.IsAssignableFrom(typeof(Attribute))) {
                throw new InvalidOperationException("'attributeType' must a valid attribute type.");   
            }

            if(memberExpression == null) {
                throw new InvalidOperationException("Expression must be a member expression");
            }
            
            return memberExpression
                .Member
                .GetCustomAttributes(false)
                .Any(a => 
                    attributeType.IsAssignableFrom(a.GetType())
                );
    	}

    	/// <summary>
        /// Composes the specified first.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="leftExpression">The left expression.</param>
        /// <param name="rightExpression">The right expression.</param>
        /// <param name="merge">The merge.</param>
        /// <returns></returns>
        public static Expression<T> Compose<T>(this Expression<T> leftExpression, Expression<T> rightExpression, Func<Expression, Expression, Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)
            var map = leftExpression.Parameters.Select((f, i) => new {f, s = rightExpression.Parameters[i]}).ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with parameters from the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, rightExpression.Body);

            // apply composition of lambda expression bodies to parameters from the first expression 
            return Expression.Lambda<T>(merge(leftExpression.Body, secondBody), leftExpression.Parameters);
        }

        /// <summary>
        /// Ands the specified left and right expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="leftExpression">The first.</param>
        /// <param name="rightExpression">The second.</param>
        /// <returns>An ANDed expression of the first and second expression.</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> leftExpression, Expression<Func<T, bool>> rightExpression)
        {
            return leftExpression.Compose(rightExpression, Expression.And);
        }

        /// <summary>
        /// Ors the specified left and right expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="leftExpression">The left expression.</param>
        /// <param name="rightExpression">The right expression.</param>
        /// <returns>An ORed expression of the first and second expression.</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> leftExpression, Expression<Func<T, bool>> rightExpression)
        {
            return leftExpression.Compose(rightExpression, Expression.Or);
        }

        /// <summary>
        /// converts the expression to text.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="thisExpression">The this expression.</param>
        /// <returns></returns>
		public static string ToText<T, TProperty>(this Expression<Func<T, TProperty>> thisExpression)
		{
			var nameParts = new Stack<string>();
			Expression part = thisExpression.Body.RemoveConvert();

			while (part != null)
			{
				if (part.NodeType == ExpressionType.Call)
				{
					var methodExpression = (MethodCallExpression)part;

					if (!IsSingleArgumentIndexer(methodExpression))
					{
						break;
					}

					nameParts.Push(
						GetIndexerInvocation(
							methodExpression.Arguments.Single(),
							thisExpression.Parameters.ToArray()
						)
					);

					part = methodExpression.Object;
				}
				else if (part.NodeType == ExpressionType.ArrayIndex)
				{
					var binaryExpression = (BinaryExpression)part;

					nameParts.Push(
						GetIndexerInvocation(
							binaryExpression.Right,
							thisExpression.Parameters.ToArray()
						)
					);

					part = binaryExpression.Left;
				}
				else if (part.NodeType == ExpressionType.MemberAccess)
				{
					var memberExpressionPart = (MemberExpression)part;
					nameParts.Push("." + memberExpressionPart.Member.Name);
					part = memberExpressionPart.Expression;
				}
				else if (part.NodeType == ExpressionType.Parameter)
				{
					nameParts.Push(String.Empty);
					part = null;
				}
				else
				{
					break;
				}
			}

			if (nameParts.Count > 0 && String.Equals(nameParts.Peek(), ".model", StringComparison.OrdinalIgnoreCase))
			{
				nameParts.Pop();
			}

			if (nameParts.Count > 0)
			{
				return nameParts.Aggregate((left, right) => left + right).TrimStart('.');
			}

			return String.Empty;
		}

		public static Expression RemoveConvert(this Expression expression)
		{
			while (expression != null && (expression.NodeType == ExpressionType.Convert || expression.NodeType == ExpressionType.ConvertChecked))
				expression = RemoveConvert(((UnaryExpression)expression).Operand);
			return expression;
		}

		private static bool IsSingleArgumentIndexer(Expression expression)
		{
			var methodExpression = expression as MethodCallExpression;
			if (methodExpression == null || methodExpression.Arguments.Count != 1)
			{
				return false;
			}

			return methodExpression.Method
								   .DeclaringType
								   .GetDefaultMembers()
								   .OfType<PropertyInfo>()
								   .Any(p => p.GetGetMethod() == methodExpression.Method);
		}

		private static string GetIndexerInvocation(Expression expression, ParameterExpression[] parameters)
		{
			Expression converted = Expression.Convert(expression, typeof(object));
			ParameterExpression fakeParameter = Expression.Parameter(typeof(object), null);
			Expression<Func<object, object>> lambda = Expression.Lambda<Func<object, object>>(converted, fakeParameter);
			Func<object, object> func;

			try
			{
				func = lambda.Compile();
			}
			catch (InvalidOperationException ex)
			{
				throw new InvalidOperationException(
					"Invalid Indexer",
					ex
				);
			}

			return "[" + Convert.ToString(func(null), CultureInfo.InvariantCulture) + "]";
		}
    }
}