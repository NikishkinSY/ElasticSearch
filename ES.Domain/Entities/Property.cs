using Newtonsoft.Json;

namespace ES.Domain.Entities
{
    public class Property: SearchItem
    {
        [JsonProperty("propertyID")]
        public int Id { get; set; }
        public string FormerName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}
