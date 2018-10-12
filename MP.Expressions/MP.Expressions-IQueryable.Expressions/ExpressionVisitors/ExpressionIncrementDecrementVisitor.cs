using System.Linq.Expressions;

namespace MP.Expressions_IQueryable.Expressions.ExpressionVisitors
{
    internal class ExpressionIncrementDecrementVisitor : ExpressionVisitor
    {
        protected override Expression VisitBinary(BinaryExpression node)
        {
            var nodeValues = ExtractNodeValues(node);

            if (IsIncrementOrDecrimentCase(nodeValues))
            {
                if (node.NodeType == ExpressionType.Add)
                {
                    return Expression.Increment(nodeValues.param);
                }

                if (node.NodeType == ExpressionType.Subtract)
                {
                    return Expression.Decrement(nodeValues.param);
                }
            }

            return base.VisitBinary(node);
        }

        #region Private methods

        private (ParameterExpression param, ConstantExpression constant) ExtractNodeValues(BinaryExpression node)
        {
            var leftNodeValues = GetNodeValues(node.Left);
            var rightNodeValues = GetNodeValues(node.Right);

            return MergeNodeRightAndLeftValues(leftNodeValues, rightNodeValues);
        }

        private (ParameterExpression param, ConstantExpression constant) GetNodeValues(Expression node)
        {
           if (node.NodeType == ExpressionType.Parameter)
           {
               return ((ParameterExpression)node, null);
           }
           else if (node.NodeType == ExpressionType.Constant)
           {
               return (null, (ConstantExpression)node);
           }

            return (null, null);
        }

        private (ParameterExpression param, ConstantExpression constant) MergeNodeRightAndLeftValues((ParameterExpression param, ConstantExpression constant) leftNodeValues, (ParameterExpression param, ConstantExpression constant) rightNodeValues)
        {
            var param = rightNodeValues.param ?? leftNodeValues.param;
            var constant = rightNodeValues.constant ?? leftNodeValues.constant;

            return (param, constant);
        }

        private bool IsIncrementOrDecrimentCase((ParameterExpression param, ConstantExpression constant) nodeValues)
        {
            return nodeValues.param != null &&
                   IsConstIntValue(nodeValues.constant) && (int)nodeValues.constant.Value == 1;
        }

        private bool IsConstIntValue(ConstantExpression constant)
        {
            return constant?.Type == typeof(int);
        }

        #endregion
    }
}
