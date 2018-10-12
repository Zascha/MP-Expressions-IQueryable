using System;

namespace MP.Expressions_IQueryable.Expressions.ExpressionVisitors
{
    internal class ExpressionVisitorsProvider
    {
        private Lazy<ExpressionIncrementDecrementVisitor> _incrementAndDecrementVisitor;
        private Lazy<ExpressionReplaceConstsVisitor> _replaceConstsVisitor;

        public ExpressionIncrementDecrementVisitor IncrementAndDecrementVisitor { get { return _incrementAndDecrementVisitor.Value; } }
        public ExpressionReplaceConstsVisitor ReplaceConstsVisitor { get { return _replaceConstsVisitor.Value; } }

        public ExpressionVisitorsProvider()
        {
            _incrementAndDecrementVisitor = new Lazy<ExpressionIncrementDecrementVisitor>();
            _replaceConstsVisitor = new Lazy<ExpressionReplaceConstsVisitor>();
        }
    }
}
