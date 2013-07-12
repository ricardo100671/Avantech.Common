﻿using System.Collections.Generic;
using System.Linq.Expressions;

namespace Avantech.Common.Linq.Expressions
{
    // TODO: Check if this cannot be simpilfied by converting each expression to text with expression extension method
    public class ExpressionEqualityComparer : IEqualityComparer<Expression>
    {
        public bool Equals(Expression x, Expression y)
        {
            return EqualsRecursive(x, y);
        }

        private bool EqualsRecursive(Expression x, Expression y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (x.GetType() != y.GetType() ||
                x.Type != y.Type ||
                x.NodeType != y.NodeType)
            {
                return false;
            }

            var lambdaExpressionX = x as LambdaExpression;
            var lambdaExpressionY = y as LambdaExpression;

            if (lambdaExpressionX != null && lambdaExpressionY != null)
            {
                return AreAllArgumentsEqual(lambdaExpressionX.Parameters, lambdaExpressionY.Parameters) &&
                    Equals(lambdaExpressionX.Body, lambdaExpressionY.Body);
            }

            var binaryExpressionX = x as BinaryExpression;
            var binaryExpressionY = y as BinaryExpression;

            if (binaryExpressionX != null && binaryExpressionY != null)
            {
                return binaryExpressionX.Method == binaryExpressionY.Method &&
                    Equals(binaryExpressionX.Left, binaryExpressionY.Left) &&
                    Equals(binaryExpressionX.Right, binaryExpressionY.Right);
            }

            var unaryExpressionX = x as UnaryExpression;
            var unaryExpressionY = y as UnaryExpression;

            if (unaryExpressionX != null && unaryExpressionY != null)
            {
                return unaryExpressionX.Method == unaryExpressionY.Method &&
                    Equals(unaryExpressionX.Operand, unaryExpressionY.Operand);
            }

            var methodCallExpressionX = x as MethodCallExpression;
            var methodCallExpressionY = y as MethodCallExpression;

            if (methodCallExpressionX != null && methodCallExpressionY != null)
            {
                return AreAllArgumentsEqual(methodCallExpressionX.Arguments, methodCallExpressionY.Arguments) &&
                    methodCallExpressionX.Method == methodCallExpressionY.Method &&
                    Equals(methodCallExpressionX.Object, methodCallExpressionY.Object);
            }

            var conditionalExpressionX = x as ConditionalExpression;
            var conditionalExpressionY = y as ConditionalExpression;

            if (conditionalExpressionX != null && conditionalExpressionY != null)
            {
                return Equals(conditionalExpressionX.Test, conditionalExpressionY.Test) &&
                    Equals(conditionalExpressionX.IfTrue, conditionalExpressionY.IfTrue) &&
                    Equals(conditionalExpressionX.IfFalse, conditionalExpressionY.IfFalse);
            }

            var invocationExpressionX = x as InvocationExpression;
            var invocationExpressionY = y as InvocationExpression;

            if (invocationExpressionX != null && invocationExpressionY != null)
            {
                return AreAllArgumentsEqual(invocationExpressionX.Arguments, invocationExpressionY.Arguments) &&
                    Equals(invocationExpressionX.Expression, invocationExpressionY.Expression);
            }

            var memberExpressionX = x as MemberExpression;
            var memberExpressionY = y as MemberExpression;

            if (memberExpressionX != null && memberExpressionY != null)
            {
                return memberExpressionX.Member == memberExpressionY.Member &&
                    Equals(memberExpressionX.Expression, memberExpressionY.Expression);
            }

            var constantExpressionX = x as ConstantExpression;
            var constantExpressionY = y as ConstantExpression;

            if (constantExpressionX != null && constantExpressionY != null)
            {
                return constantExpressionX.Value.Equals(constantExpressionY.Value);
            }

            var parameterExpressionX = x as ParameterExpression;
            var parameterExpressionY = y as ParameterExpression;

            if (parameterExpressionX != null && parameterExpressionY != null)
            {
                return parameterExpressionX.Name == parameterExpressionY.Name;
            }

            var newExpressionX = x as NewExpression;
            var newExpressionY = y as NewExpression;

            if (newExpressionX != null && newExpressionY != null)
            {
                return AreAllArgumentsEqual(newExpressionX.Arguments, newExpressionY.Arguments) &&
                    newExpressionX.Constructor == newExpressionY.Constructor;
            }

            //MemberInitExpression memberInitExpressionX = x as MemberInitExpression;
            //MemberInitExpression memberInitExpressionY = y as MemberInitExpression;

            //if (memberInitExpressionX != null && memberExpressionY != null)
            //{
            //    return Equals(memberInitExpressionX.NewExpression, memberInitExpressionY.NewExpression) &&
            //        memberInitExpressionX.Bindings.All(binding => binding is MemberAssignment) &&
            //        memberInitExpressionY.Bindings.All(binding => binding is MemberAssignment) &&
            //        AreAllArgumentsEqual(memberInitExpressionX.Bindings.Select(binding => (MemberAssignment)binding.Member));

            //}

            return false;
        }
        private bool AreAllArgumentsEqual<T>(IEnumerable<T> xArguments, IEnumerable<T> yArguments)
            where T : Expression
        {
            var argumentEnumeratorX = xArguments.GetEnumerator();
            var argumentEnumeratorY = yArguments.GetEnumerator();

            var haveNotEnumeratedAllOfX = argumentEnumeratorX.MoveNext();
            var haveNotEnumeratedAllOfY = argumentEnumeratorY.MoveNext();

            var areAllArgumentsEqual = true;
            while (haveNotEnumeratedAllOfX && haveNotEnumeratedAllOfY && areAllArgumentsEqual)
            {
                areAllArgumentsEqual = Equals(argumentEnumeratorX.Current, argumentEnumeratorY.Current);
                haveNotEnumeratedAllOfX = argumentEnumeratorX.MoveNext();
                haveNotEnumeratedAllOfY = argumentEnumeratorY.MoveNext();
            }

            if (haveNotEnumeratedAllOfX || haveNotEnumeratedAllOfY)
            {
                return false;
            }

            return areAllArgumentsEqual;
        }

        public int GetHashCode(Expression x)
        {
            var lambdaExpressionX = x as LambdaExpression;

            if (lambdaExpressionX != null)
            {
                return XorHashCodes(lambdaExpressionX.Parameters) ^
                    GetHashCode(lambdaExpressionX.Body);
            }

            var binaryExpressionX = x as BinaryExpression;

            if (binaryExpressionX != null)
            {
                int methodHashCode = binaryExpressionX.Method != null ? binaryExpressionX.Method.GetHashCode() : binaryExpressionX.NodeType.GetHashCode();
                return methodHashCode ^
                    GetHashCode(binaryExpressionX.Left) ^
                    GetHashCode(binaryExpressionX.Right);
            }

            var unaryExpressionX = x as UnaryExpression;

            if (unaryExpressionX != null)
            {
                int methodHashCode = unaryExpressionX.Method != null ? unaryExpressionX.Method.GetHashCode() : unaryExpressionX.NodeType.GetHashCode();
                return methodHashCode ^
                    GetHashCode(unaryExpressionX.Operand);
            }

            var methodCallExpressionX = x as MethodCallExpression;

            if (methodCallExpressionX != null)
            {
                return XorHashCodes(methodCallExpressionX.Arguments) ^
                    methodCallExpressionX.Method.GetHashCode() ^
                    GetHashCode(methodCallExpressionX.Object);
            }

            var conditionalExpressionX = x as ConditionalExpression;

            if (conditionalExpressionX != null)
            {
                return
                    GetHashCode(conditionalExpressionX.Test) ^
                    GetHashCode(conditionalExpressionX.IfTrue) ^
                    GetHashCode(conditionalExpressionX.IfFalse);
            }

            var invocationExpressionX = x as InvocationExpression;

            if (invocationExpressionX != null)
            {
                return
                    XorHashCodes(invocationExpressionX.Arguments) ^
                    GetHashCode(invocationExpressionX.Expression);
            }

            var memberExpressionX = x as MemberExpression;

            if (memberExpressionX != null)
            {
                return
                    memberExpressionX.Member.GetHashCode() ^
                    GetHashCode(memberExpressionX.Expression);
            }

            var constantExpressionX = x as ConstantExpression;

            if (constantExpressionX != null)
            {
                int valueHash = constantExpressionX.Value != null ? constantExpressionX.Value.GetHashCode() : constantExpressionX.GetHashCode();
                return valueHash;
            }

            var newExpressionX = x as NewExpression;

            if (newExpressionX != null)
            {

                return
                    XorHashCodes(newExpressionX.Arguments) ^
                    newExpressionX.Constructor.GetHashCode();
            }

            return 0;
        }

        private int XorHashCodes<T>(IEnumerable<T> expressions)
            where T : Expression
        {
            var accumulatedHashCode = 0;
            foreach (var expression in expressions)
            {
                accumulatedHashCode ^= GetHashCode(expression);
            }

            return accumulatedHashCode;
        }
    }
}
