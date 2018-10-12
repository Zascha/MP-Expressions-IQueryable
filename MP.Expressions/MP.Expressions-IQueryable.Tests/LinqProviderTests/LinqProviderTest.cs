using FakeItEasy;
using MP.Expressions_IQueryable.LinqProvider;
using MP.Expressions_IQueryable.LinqProvider.E3SClient;
using MP.Expressions_IQueryable.LinqProvider.E3SClient.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MP.Expressions_IQueryable.Tests.LinqProviderTests
{
    public class LinqProviderTest
    {
        private readonly E3SQueryClient _client;
        private readonly E3SEntitySet<EmployeeEntity> _employees;

        public LinqProviderTest()
        {
            _client = A.Fake<E3SQueryClient>();
            _employees = new E3SEntitySet<EmployeeEntity>(_client);

            A.CallTo(() => _client.SearchFTS(A<Type>._, A<IEnumerable<string>>._, A<int>._, A<int>._)).Returns(Enumerable.Empty<EmployeeEntity>());
        }

        [Fact]
        public void Where_Equals_RightOrderOfParams_Test()
        {
            var expectedQueryString = "workstation:(EPRUIZHW006)";
            var expectedRequestQueries = new string[] { expectedQueryString };

            var filteredEmployees = _employees.Where(e => e.workstation == "EPRUIZHW006").ToList();

            A.CallTo(() => _client.SearchFTS(A<Type>._, A<IEnumerable<string>>.That.Matches(array => array.SequenceEqual(expectedRequestQueries)), A<int>._, A<int>._))
                                  .MustHaveHappened();
        }

        [Fact]
        public void Where_Equals_RevertOrderOfParams_Test()
        {
            var expectedQueryString = "workstation:(EPRUIZHW006)";
            var expectedRequestQueries = new string[] { expectedQueryString };

            var filteredEmployees = _employees.Where(e => "EPRUIZHW006" == e.workstation).ToList();

            A.CallTo(() => _client.SearchFTS(A<Type>._, A<IEnumerable<string>>.That.Matches(array => array.SequenceEqual(expectedRequestQueries)), A<int>._, A<int>._))
                                  .MustHaveHappened();
        }

        [Fact]
        public void Where_StartsWith_Test()
        {
            var expectedQueryString = "workstation:(EPR*)";
            var expectedRequestQueries = new string[] { expectedQueryString };

            var filteredEmployees = _employees.Where(e => e.workstation.StartsWith("EPR")).ToList();

            A.CallTo(() => _client.SearchFTS(A<Type>._, A<IEnumerable<string>>.That.Matches(array => array.SequenceEqual(expectedRequestQueries)), A<int>._, A<int>._))
                                  .MustHaveHappened();
        }

        [Fact]
        public void Where_EndsWith_Test()
        {
            var expectedQueryString = "workstation:(*006)";
            var expectedRequestQueries = new string[] { expectedQueryString };

            var filteredEmployees = _employees.Where(e => e.workstation.EndsWith("006")).ToList();

            A.CallTo(() => _client.SearchFTS(A<Type>._, A<IEnumerable<string>>.That.Matches(array => array.SequenceEqual(expectedRequestQueries)), A<int>._, A<int>._))
                                  .MustHaveHappened();
        }

        [Fact]
        public void Where_Contains_Test()
        {
            var expectedQueryString = "workstation:(*RUIZ*)";
            var expectedRequestQueries = new string[] { expectedQueryString };

            var filteredEmployees = _employees.Where(e => e.workstation.Contains("RUIZ")).ToList();

            A.CallTo(() => _client.SearchFTS(A<Type>._, A<IEnumerable<string>>.That.Matches(array => array.SequenceEqual(expectedRequestQueries)), A<int>._, A<int>._))
                                  .MustHaveHappened();
        }

        [Fact]
        public void Where_AndOperator_Test()
        {
            var expectedQueryStartWithString = "workstation:(EPR*)";
            var expectedQueryEndsWithString = "workstation:(*006)";
            var expectedRequestQueries = new string[] { expectedQueryStartWithString, expectedQueryEndsWithString };

            var filteredEmployees = _employees.Where(e => e.workstation.StartsWith("EPR") && e.workstation.EndsWith("006")).ToList();

            A.CallTo(() => _client.SearchFTS(A<Type>._, A<IEnumerable<string>>.That.Matches(array => array.SequenceEqual(expectedRequestQueries)), A<int>._, A<int>._))
                                  .MustHaveHappened();
        }
    }
}
