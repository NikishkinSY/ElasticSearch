using Nest;
using Newtonsoft.Json;

namespace ES.Application.ElasticSearch.Entities
{
    [ElasticsearchType(IdProperty = nameof(ESId))]
    public class ManagementES: IESEntity
    {
        public string ESId { get { return $"{Id}{Market}"; } }
        [JsonProperty("mgmtID")]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Market { get; set; }
        public string State { get; set; }
    }
}
