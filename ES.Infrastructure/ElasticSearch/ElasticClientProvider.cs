using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Elasticsearch.Net;
using Elasticsearch.Net.Aws;
using ES.Domain.Configuration;
using ES.Domain.Exceptions;
using Microsoft.Extensions.Options;
using Nest;
using Nest.JsonNetSerializer;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace ElasticsearchRecipes.Elastic
{
    public class ElasticClientProvider
    {
        private ElasticClient _client { get; set; }
        private ElasticSearchSettings _esSettings { get; set; }
        private AWSSettings _awsSettings { get; set; }

        public ElasticClientProvider(
            IOptions<ElasticSearchSettings> esSettings,
            IOptions<AWSSettings> awsSettings)
        {
            _esSettings = esSettings.Value;
            _awsSettings = awsSettings.Value;
        }

        public ElasticClient Get()
        {
            if (_client == null)
            {
                var chain = new CredentialProfileStoreChain(_awsSettings.AWSProfilesLocation);
                if (chain.TryGetAWSCredentials(_awsSettings.AWSProfileName, out AWSCredentials awsCredentials))
                {
                    var httpConnection = new AwsHttpConnection(awsCredentials, RegionEndpoint.USEast2);
                    var pool = new StaticConnectionPool(_esSettings.Url.Split(',').Select(p => new Uri(p)));
                    var connection = new ConnectionSettings(pool, httpConnection,
                        sourceSerializer: (b, s) => new JsonNetSerializer(b, s,
                            () => new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }));
                    _client = new ElasticClient(connection);// Use awsCredentials
                }
                else
                {
                    throw new NotFoundException("File with aws credentilas not found");
                }
            }

            return _client;
        }
    }
}