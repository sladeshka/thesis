using Newtonsoft.Json;

namespace SalesVentilationEquipment.Server.Responses
{
    [JsonObject]
    public class ProductResponse
    {
        #region Fields
        [JsonProperty("id")]
        public required Guid Id { get; set; }

        [JsonProperty("name")]
        public required string Name { get; set; }

        [JsonProperty("price")]
        public required double Price { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("feature")]
        public string? Feature { get; set; }
        #endregion
    }
}