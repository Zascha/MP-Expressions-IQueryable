using MP.Expressions_IQueryable.Mapper.Models;
using MP.Expressions_IQueryable.Mapper;
using Xunit;

namespace MP.Expressions_IQueryable.Tests.MappingTests
{
    public class MapperTests
    {
        private readonly MappingGenerator _mappingGenerator;
        private readonly UserBLL _user;

        public MapperTests()
        {
            _mappingGenerator = new MappingGenerator();
            _user = new UserBLL { Id = 10, FirstName = "Dipper", LastName = "Pines", Age = 14 };
        }

        [Fact]
        public void Map_WithNoMapperSettings_TheSameTypePropertiesNames_ReturnsMappedObject()
        {
            var mapper = _mappingGenerator.Generate<UserBLL, UserPL>();
            var actualResult = mapper.Map(_user);

            Assert.NotNull(actualResult);
            Assert.IsType<UserPL>(actualResult);
            Assert.Equal(_user.Id, actualResult.Id);
            Assert.Equal(_user.FirstName, actualResult.FirstName);
            Assert.Equal(_user.LastName, actualResult.LastName);
            Assert.Equal(_user.Age, actualResult.Age);
        }

        [Fact]
        public void Map_WithNoMapperSettings_DifferentTypePropertiesNames_ReturnsMappedObjectWithNullValues()
        {
            var mapper = _mappingGenerator.Generate<UserBLL, UserDAL>();
            var actualResult = mapper.Map(_user);

            Assert.NotNull(actualResult);
            Assert.IsType<UserDAL>(actualResult);
            Assert.Equal(_user.Id, actualResult.Id);
            Assert.Null(actualResult.Name);
            Assert.Null(actualResult.Surname);
            Assert.Equal(default(int), actualResult.YearsOld);
        }
    }
}
