using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Elasticsearch.Net;
using Elasticsearch.Net.Aws;
using ES.Domain.Configuration;
using ES.Domain.Exceptions;
using ES.Infrastructure.AWS.Interfaces;
using Microsoft.Extensions.Options;

namespace ES.Infrastructure.AWS
{
    public class AWSConnectionProvider: IAWSConnectionProvider
    {
        private AWSSettings _awsSettings { get; set; }

        public AWSConnectionProvider(IOptionsMonitor<AWSSettings> awsSettings)
        {
            _awsSettings = awsSettings.CurrentValue;
        }

        public IConnection Create()
        {
            var chain = new CredentialProfileStoreChain(_awsSettings.AWSProfilesLocation);
            if (chain.TryGetAWSCredentials(_awsSettings.AWSProfileName, out AWSCredentials awsCredentials))
            {
                return new AwsHttpConnection(awsCredentials, RegionEndpoint.GetBySystemName(_awsSettings.RegionEndpoint));
            }
            else
            {
                throw new NotFoundException("File with aws credentilas not found");
            }
        }
    }
}
