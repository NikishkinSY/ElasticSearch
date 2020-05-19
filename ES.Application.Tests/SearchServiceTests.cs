using AutoMapper;
using ES.Application.ElasticSearch.Entities;
using ES.Application.Services;
using ES.Application.Services.Interfaces;
using ES.Domain.Entities;
using ES.Infrastructure.ElasticSearch.Interfaces;
using Moq;
using Nest;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ES.Application.Tests
{
    public class SearchServiceTests
    {
        private ISearchService _searchService { get; set; }

        internal void Init()
        {
            var searchResponseMock = new Mock<ISearchResponse<object>>();
            searchResponseMock.SetupGet(x => x.IsValid).Returns(true);
            searchResponseMock.SetupGet(x => x.Documents).Returns(new List<object>() { new ManagementES(), new PropertyES() });

            var elasticClientMock = new Mock<IElasticClient>();
            elasticClientMock.Setup(a => a.SearchAsync<object>(It.IsAny<ISearchRequest<BaseItem>>(), default))
                .ReturnsAsync(searchResponseMock.Object);

            var elasticClientProviderMock = new Mock<IElasticClientProvider>();
            elasticClientProviderMock.Setup(a => a.Get()).Returns(elasticClientMock.Object);

            var mappingConfig = new MapperConfiguration(cfg =>
                cfg.AddProfile(typeof(AppServicesProfile))
            );
            var mapper = mappingConfig.CreateMapper();

            _searchService = new SearchService(elasticClientProviderMock.Object, mapper);
        }

        [Fact]
        public void Search_Configurate_Successful()
        {
            // arrange
            Init();

            // act
            var result = _searchService.SearchAsync(string.Empty, new List<string>(), new List<string>(), default).Result;

            // assert
            Assert.Equal(2, result.Count());
        }
    }
}
