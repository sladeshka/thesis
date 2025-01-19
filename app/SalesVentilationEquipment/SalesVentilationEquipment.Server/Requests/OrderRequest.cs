using Newtonsoft.Json;

namespace SalesVentilationEquipment.Server.Requests
{
    public class OrderRequest
    {
        #region Fields
        [JsonProperty("id")]
        public required Guid Id { get; set; }

        [JsonProperty("contractorId")]
        public required Guid ContractorId { get; set; }

        [JsonProperty("cartId")]
        public required Guid CartId { get; set; }

        [JsonProperty("orderStatus")]
        public required string OrderStatus { get; set; }
        #endregion
    }
}
