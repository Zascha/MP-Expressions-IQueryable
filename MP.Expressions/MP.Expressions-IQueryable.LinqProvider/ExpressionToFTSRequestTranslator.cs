using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MP.Expressions_IQueryable.LinqProvider
{
    public class ExpressionToFTSRequestTranslator : ExpressionVisitor
    {
        private readonly List<string> requestQueries;
        private StringBuilder stringBuilder;

        public ExpressionToFTSRequestTranslator()
        {
            requestQueries = new List<string>();
            stringBuilder = new StringBuilder();
        }

        public IEnumerable<string> Translate(Expression exp)
        {
            Visit(exp);

            requestQueries.Add(stringBuilder.ToString());

            return requestQueries;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(string))
            {
                return HandleForStringMethodDeclaringType(node);
            }

            if (node.Method.DeclaringType == typeof(Queryable)
                && node.Method.Name == "Where")
            {
                var predicate = node.Arguments[1];
                Visit(predicate);

                return node;
            }

            return base.VisitMethodCall(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                    if (!IsNodeEqualityConditionValid(node))
                    {
                        throw new NotSupportedException(string.Format("Operands should be a property and a field", node.NodeType));
                    }

                    var memberAccessNode = GetChildrenNodeOfRequiredType(node, ExpressionType.MemberAccess);
                    var constantNode = GetChildrenNodeOfRequiredType(node, ExpressionType.Constant);

                    Visit(memberAccessNode);
                    stringBuilder.Append("(");
                    Visit(constantNode);
                    stringBuilder.Append(")");
                    break;

                case ExpressionType.AndAlso:
                    Visit(node.Left);
                    requestQueries.Add(stringBuilder.ToString());
                    stringBuilder = new StringBuilder();
                    Visit(node.Right);
                    break;

                default:
                    throw new NotSupportedException(string.Format("Operation {0} is not supported", node.NodeType));
            };

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            stringBuilder.Append(node.Member.Name).Append(":");

            return base.VisitMember(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            stringBuilder.Append(node.Value);

            return node;
        }

        #region Private methods

        private bool IsNodeEqualityConditionValid(BinaryExpression node)
        {
            if (node.Left.NodeType == ExpressionType.Constant && node.Right.NodeType == ExpressionType.MemberAccess ||
                node.Right.NodeType == ExpressionType.Constant && node.Left.NodeType == ExpressionType.MemberAccess)
            {
                return true;
            }

            return false;
        }

        private Expression GetChildrenNodeOfRequiredType(BinaryExpression node, ExpressionType expressionType)
        {
            if (node.Left.NodeType == expressionType)
            {
                return node.Left;
            }

            if (node.Right.NodeType == expressionType)
            {
                return node.Right;
            }

            throw new InvalidOperationException("Passed node does not contain a child node of required type.");
        }

        private Expression HandleForStringMethodDeclaringType(MethodCallExpression node)
        {
            if (node.Object.NodeType != ExpressionType.MemberAccess)
            {
                throw new ArgumentException("Passed node should have a node of MemberAccess type.");
            }

            Visit(node.Object);
            stringBuilder.Append("(");

            if (node.Method.Name == "EndsWith" || node.Method.Name == "Contains")
            {
                Visit(Expression.Constant("*"));
            }

            Visit(node.Arguments.First());

            if (node.Method.Name == "StartsWith" || node.Method.Name == "Contains")
            {
                Visit(Expression.Constant("*"));
            }

            stringBuilder.Append(")");

            return node;
        }

        #endregion
    }
}
