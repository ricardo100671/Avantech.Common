
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Avantech.Common.Linq.Expressions
{
    public class ParameterRebinder : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> _map;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterRebinder"/> class.
        /// </summary>
        /// <param name="map">The parameter map.</param>
        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            _map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        /// <summary>
        /// Replaces the parameters.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression expression)
        {
            return new ParameterRebinder(map).Visit(expression);
        }

        /// <summary>
        /// Visits the parameter in the expression tree map.
        /// </summary>
        /// <param name="parameterExpression">The parameter expression.</param>
        /// <returns></returns>
        protected override Expression VisitParameter(ParameterExpression parameterExpression)
        {
            ParameterExpression replacement;
            if (_map.TryGetValue(parameterExpression, out replacement))
            {
                parameterExpression = replacement;
            }
            return base.VisitParameter(parameterExpression);
        }
    }
}