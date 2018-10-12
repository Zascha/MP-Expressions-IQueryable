using MP.Expressions_IQueryable.Expressions.ExpressionVisitors;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MP.Expressions_IQueryable.Expressions
{
    public static class ExpressionTransformator
    {
        private static ExpressionVisitorsProvider _visitors = new ExpressionVisitorsProvider();

        /// <summary>
        /// Replaces expressions like '[variable] + 1' / '[variable] - 1' with increment and decrement operations
        /// </summary>
        /// <param name="expression">Expression to modify</param>
        /// <returns>Expression with increment and decrement operations</returns>
        public static Expression EmbedIncrementAndDecrement(this Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return _visitors.IncrementAndDecrementVisitor.VisitAndConvert(expression, string.Empty);
        }

        /// <summary>
        /// Replaces expression parameters with passed constants
        /// </summary>
        /// <param name="expression">Expression to modify</param>
        /// <param name="replacingMap">Dictionary of pairs key - param name, value - const value to replace param</param>
        /// <returns>Expression with constants</returns>
        public static Expression ReplaceConsts(this Expression expression, Dictionary<string, object> replacingMap)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (replacingMap == null)
                throw new ArgumentNullException(nameof(replacingMap));

            _visitors.ReplaceConstsVisitor.ReplaceConstsMap = replacingMap;

            return _visitors.ReplaceConstsVisitor.VisitAndConvert(expression, string.Empty);
        }
    }
}
