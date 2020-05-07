using Newtonsoft.Json;

namespace ES.Domain.Entities
{
    public class Management: SearchItem
    {
        [JsonProperty("mgmtID")]
        public int Id { get; set; }
    }
}
