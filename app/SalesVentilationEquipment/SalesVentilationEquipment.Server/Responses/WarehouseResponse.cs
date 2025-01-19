using Newtonsoft.Json;

namespace SalesVentilationEquipment.Server.Responses
{
    public class WarehouseResponse
    {
        #region Fields
        [JsonProperty("id")]
        public required Guid Id { get; set; } = Guid.NewGuid();

        [JsonProperty("name")]
        public required string Name { get; set; }

        [JsonProperty("address")]
        public required string Address { get; set; }
        #endregion
    }
}
