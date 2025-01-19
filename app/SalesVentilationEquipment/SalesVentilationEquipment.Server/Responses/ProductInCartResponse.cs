using Newtonsoft.Json;

namespace SalesVentilationEquipment.Server.Responses
{
    [JsonObject]
    public class ProductInCartResponse
    {
        #region Fields
        [JsonProperty("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonProperty("cart_id")]
        public required Guid CartId { get; set; }
        
        [JsonProperty("product_id")]
        public required Guid ProductId { get; set; }

        [JsonProperty("unit_price")]
        public required double UnitPrice { get; set; }

        [JsonProperty("quantity")]
        public required double Quantity { get; set; }
        #endregion
    }
}
