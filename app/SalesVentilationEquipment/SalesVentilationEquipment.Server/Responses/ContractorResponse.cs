using Newtonsoft.Json;

namespace SalesVentilationEquipment.Server.Responses
{
    [JsonObject]
    public class ContractorResponse
    {
        #region Fields
        [JsonProperty("id")]
        public required Guid Id { get; set; } = Guid.NewGuid();

        [JsonProperty("name")]
        public required string Name { get; set; }

        [JsonProperty("contact_info")]
        public required string ContactInfo { get; set; }
        #endregion
    }
}
