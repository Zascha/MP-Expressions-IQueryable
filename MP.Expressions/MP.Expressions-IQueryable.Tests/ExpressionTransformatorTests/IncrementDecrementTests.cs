using ContinuousLinq.Expressions;
using MP.Expressions_IQueryable.Expressions;
using System;
using System.Linq.Expressions;
using Xunit;

namespace MP.Expressions_IQueryable.Tests.ExpressionTransformatorTests
{
    public class IncrementDecrementTests
    {
        private readonly ExpressionEqualityComparer _expressionEqualityComparer;

        public IncrementDecrementTests()
        {
            _expressionEqualityComparer = new ExpressionEqualityComparer();
        }

        [Fact]
        public void EmbedIncrementAndDecrement_TestIncrement_ValidIncrementCondition_ReplacesWithIncrementOperations()
        {
            Expression<Func<int, int>> expression = i => i + 1;

            var expectedResult = Expression.Lambda(typeof(Func<int, int>), Expression.Increment(Expression.Parameter(typeof(int), "i")), Expression.Parameter(typeof(int), "i"));
            var actualResult = expression.EmbedIncrementAndDecrement();

            Assert.NotNull(actualResult);
            Assert.True(_expressionEqualityComparer.Equals(expectedResult, actualResult));
        }

        [Fact]
        public void EmbedIncrementAndDecrement_TestIncrement_InValidIncrementCondition_ReplacesWithIncrementOperations()
        {
            Expression<Func<int, int>> expression = i => i + 2;

            var expectedResult = Expression.Lambda(typeof(Func<int, int>), Expression.Increment(Expression.Parameter(typeof(int), "i")), Expression.Parameter(typeof(int), "i"));
            var actualResult = expression.EmbedIncrementAndDecrement();

            Assert.NotNull(actualResult);
            Assert.False(_expressionEqualityComparer.Equals(expectedResult, actualResult));
        }

        [Fact]
        public void EmbedIncrementAndDecrement_TestDecrement_ValidDecrementCondition_ReplacesWithIncrementOperations()
        {
            Expression<Func<int, int>> expression = i => i - 1;

            var expectedResult = Expression.Lambda(typeof(Func<int, int>), Expression.Decrement(Expression.Parameter(typeof(int), "i")), Expression.Parameter(typeof(int), "i"));
            var actualResult = expression.EmbedIncrementAndDecrement();

            Assert.NotNull(actualResult);
            Assert.True(_expressionEqualityComparer.Equals(expectedResult, actualResult));
        }

        [Fact]
        public void EmbedIncrementAndDecrement_TestDecrement_InValidDecrementCondition_ReplacesWithIncrementOperations()
        {
            Expression<Func<int, int>> expression = i => i - 2;

            var expectedResult = Expression.Lambda(typeof(Func<int, int>), Expression.Increment(Expression.Parameter(typeof(int), "i")), Expression.Parameter(typeof(int), "i"));
            var actualResult = expression.EmbedIncrementAndDecrement();

            Assert.NotNull(actualResult);
            Assert.False(_expressionEqualityComparer.Equals(expectedResult, actualResult));
        }

        [Fact]
        public void EmbedIncrementAndDecrement_TestCompicatedCase()
        {
            Expression<Func<int, int>> expression = i => (i - 1) + (i + 1) + i;

            var actualResult = expression.EmbedIncrementAndDecrement();

            Assert.NotNull(actualResult);
        }
    }
}
