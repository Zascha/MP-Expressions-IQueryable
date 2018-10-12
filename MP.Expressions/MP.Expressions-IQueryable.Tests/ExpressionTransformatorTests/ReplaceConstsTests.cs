using ContinuousLinq.Expressions;
using MP.Expressions_IQueryable.Expressions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace MP.Expressions_IQueryable.Tests.ExpressionTransformatorTests
{
    public class ReplaceConstsTests
    {
        private readonly ExpressionEqualityComparer _expressionEqualityComparer;

        private readonly Dictionary<string, object> _replaceMap;

        public ReplaceConstsTests()
        {
            _expressionEqualityComparer = new ExpressionEqualityComparer();
            _replaceMap = new Dictionary<string, object> { { "x", 2 }, { "y", "string" } };
        }

        [Fact]
        public void ReplaceConsts_IntParamIsInReplaceMap_RepcacesParamWithConstFromReplaceMap()
        {
            Expression<Action<int>> expression = x => Console.Write(x);
            Expression<Action> expectedResult = () => Console.Write(2);

            var actualResult = expression.ReplaceConsts(_replaceMap);

            Assert.NotNull(actualResult);
            Assert.True(_expressionEqualityComparer.Equals(expectedResult, actualResult));
        }

        [Fact]
        public void ReplaceConsts_IntParamIsNotInReplaceMap_DoesNotRepcaceParamWithConstFromReplaceMap()
        {
            Expression<Action<int>> expression = a => Console.Write(a);
            Expression<Action> expectedResult = () => Console.Write(2);

            var actualResult = expression.ReplaceConsts(_replaceMap);

            Assert.NotNull(actualResult);
            Assert.False(_expressionEqualityComparer.Equals(expectedResult, actualResult));
        }

        [Fact]
        public void ReplaceConsts_StringParamIsInReplaceMap_RepcacesParamWithConstFromReplaceMap()
        {
            Expression<Action<string>> expression = y => Console.WriteLine(y);
            Expression<Action> expectedResult = () => Console.WriteLine("string");

            var actualResult = expression.ReplaceConsts(_replaceMap);

            Assert.NotNull(actualResult);
            Assert.True(_expressionEqualityComparer.Equals(expectedResult, actualResult));
        }

        [Fact]
        public void ReplaceConsts_StringParamIsNotInReplaceMap_DoesNotRepcaceParamWithConstFromReplaceMap()
        {
            Expression<Action<string>> expression = a => Console.WriteLine(a);
            Expression<Action> expectedResult = () => Console.WriteLine("string");

            var actualResult = expression.ReplaceConsts(_replaceMap);

            Assert.NotNull(actualResult);
            Assert.False(_expressionEqualityComparer.Equals(expectedResult, actualResult));
        }

        [Fact]
        public void ReplaceConsts_TestCompicatedCase()
        {
            Expression<Action<int, string, object>> expression = (x, y, z) => string.Concat(x.ToString(), y, z.ToString());
            Expression<Action<object>> expectedResult = (z) => string.Concat(2.ToString(), "string", z.ToString());

            var actualResult = expression.ReplaceConsts(_replaceMap);

            Assert.NotNull(actualResult);
            Assert.True(_expressionEqualityComparer.Equals(expectedResult, actualResult));
        }
    }
}
