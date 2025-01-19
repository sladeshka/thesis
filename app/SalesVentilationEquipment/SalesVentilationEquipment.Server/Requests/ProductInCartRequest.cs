using Newtonsoft.Json;

namespace SalesVentilationEquipment.Server.Requests
{
    [JsonObject]
    public class ProductInCartRequest
    {
        #region Fields
        [JsonProperty("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonProperty("product_id")]
        public required Guid ProductId { get; set; }

        [JsonProperty("quantity")]
        public required double Quantity { get; set; }
        #endregion
    }
}
