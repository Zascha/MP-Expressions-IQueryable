using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MP.Expressions_IQueryable.Mapper
{
    public class MappingGenerator
    {
        public Mapper<TInputType, TOutputType> Generate<TInputType, TOutputType>()
        {
            var input = Expression.Parameter(typeof(TInputType));

            var mapFunction = Expression.Lambda<Func<TInputType, TOutputType>>(FormMapLambdaFunction(input, typeof(TOutputType)), input);

            return new Mapper<TInputType, TOutputType>(mapFunction.Compile());
        }

        #region Private methods

        private Expression FormMapLambdaFunction(ParameterExpression inputTypeObject, Type outputType)
        {
            var bindings = new List<MemberAssignment>();

            var inputTypeProperties = inputTypeObject.Type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
            var outputTypeProperties = outputType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);

            foreach(var outputPoperty in outputTypeProperties)
            {
                var correlatingInputProperty = inputTypeProperties.SingleOrDefault(property => property.Name.Equals(outputPoperty.Name, StringComparison.InvariantCultureIgnoreCase));

                if (correlatingInputProperty != null)
                {
                    bindings.Add(Expression.Bind(outputPoperty, Expression.Property(inputTypeObject, correlatingInputProperty)));
                }
            }

            return Expression.MemberInit(Expression.New(outputType), bindings);
        }

        #endregion
    }
}
