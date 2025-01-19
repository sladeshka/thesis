using Newtonsoft.Json;

namespace SalesVentilationEquipment.Server.Requests
{
    [JsonObject]
    public class CartRequest
    {
        #region Fields
        [JsonProperty("id")]
        public required Guid Id { get; set; }

        [JsonProperty("contractorId", Required = Required.Always)]
        public required Guid ContractorId { get; set; }
        #endregion
    }
}
