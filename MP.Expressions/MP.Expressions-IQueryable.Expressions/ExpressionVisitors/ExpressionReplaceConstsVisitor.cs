using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MP.Expressions_IQueryable.Expressions.ExpressionVisitors
{
    internal class ExpressionReplaceConstsVisitor : ExpressionVisitor
    {
        private Dictionary<string, object> _replaceConstsMap;

        public Dictionary<string, object> ReplaceConstsMap
        {
            set { _replaceConstsMap = value ?? throw new ArgumentNullException(nameof(value)); }
        }

        protected override Expression VisitLambda<T> (Expression<T> expression)
        {
            var replaceParams = expression.Parameters.Where(param => _replaceConstsMap.ContainsKey(param.Name));

            if (!replaceParams.Any())
            {
                return expression;
            }

            var inputParams = expression.Parameters.Except(replaceParams).ToArray();
            var inputParamsTypes = inputParams.Select(param => param.Type).ToArray();
            var returnType = typeof(void);

            if(expression.ReturnType == typeof(void))
            {
                returnType = Expression.GetActionType(inputParamsTypes);
            }
            else
            {
                inputParamsTypes = inputParamsTypes.Concat(new List<Type> { expression.ReturnType }).ToArray();
                returnType = Expression.GetFuncType(inputParamsTypes);
            }

            var lambdaBody = VisitAndConvert(expression.Body, string.Empty);

            return Expression.Lambda(returnType, lambdaBody, inputParams);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if(_replaceConstsMap.ContainsKey(node.Name) && 
               _replaceConstsMap[node.Name].GetType() == node.Type)
            {
                return Expression.Constant(_replaceConstsMap[node.Name]);
            }

            return base.VisitParameter(node);
        }
    }
}
