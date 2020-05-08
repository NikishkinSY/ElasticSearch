using Nest;
using Newtonsoft.Json;

namespace ES.Application.ElasticSearch.Entities
{
    [ElasticsearchType(IdProperty = nameof(ESId))]
    public class PropertyES: IESEntity
    {
        public string ESId { get { return Id.ToString(); } }
        [JsonProperty("propertyID")]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Market { get; set; }
        public string State { get; set; }
        public string FormerName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}
