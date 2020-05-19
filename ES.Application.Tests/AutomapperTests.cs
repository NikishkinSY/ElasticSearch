using AutoMapper;
using ES.Application.ElasticSearch.Entities;
using ES.Domain.Entities;
using System.Collections.Generic;
using Xunit;

namespace ES.Application.Tests
{
    public class AutomapperTests
    {
        private MapperConfiguration _mapperConfiguration { get; set; }

        internal void Init()
        {
            _mapperConfiguration = new MapperConfiguration(cfg =>
                cfg.AddProfile(typeof(AppServicesProfile))
            );
        }

        [Fact]
        public void Configuration_Is_Valid()
        {
            // arrange
            Init();

            // act

            // assert
            _mapperConfiguration.AssertConfigurationIsValid();
        }

        [Theory]
        [MemberData(nameof(GetManagementES))]
        public void Management_Map_Successful(ManagementES entity)
        {
            // arrange
            Init();

            // act
            var mapper = _mapperConfiguration.CreateMapper();
            var result = mapper.Map<Management>(entity);

            // assert
            Assert.Equal(entity.Id, result.Id);
            Assert.Equal(entity.Name, result.Name);
            Assert.Equal(entity.Market, result.Market);
            Assert.Equal(entity.State, result.State);
        }

        [Theory]
        [MemberData(nameof(GetPropertyES))]
        public void Property_Map_Successful(PropertyES entity)
        {
            // arrange
            Init();

            // act
            var mapper = _mapperConfiguration.CreateMapper();
            var result = mapper.Map<Property>(entity);

            // assert
            Assert.Equal(entity.Id, result.Id);
            Assert.Equal(entity.Name, result.Name);
            Assert.Equal(entity.Market, result.Market);
            Assert.Equal(entity.State, result.State);
            Assert.Equal(entity.FormerName, result.FormerName);
            Assert.Equal(entity.StreetAddress, result.StreetAddress);
            Assert.Equal(entity.City, result.City);
            Assert.Equal(entity.Lat, result.Lat);
            Assert.Equal(entity.Lng, result.Lng);
        }

        public static IEnumerable<object[]> GetManagementES()
        {
            yield return new object[] { new ManagementES() { Id = 1, Name = "Name", Market = "Market", State = "State" } };
        }

        public static IEnumerable<object[]> GetPropertyES()
        {
            yield return new object[] { new PropertyES() { Id = 1, Name = "Name", Market = "Market", State = "State", FormerName = "FormerName", StreetAddress = "StreetAddress", City = "City", Lat = 1, Lng = 1 } };
        }
    }
}
