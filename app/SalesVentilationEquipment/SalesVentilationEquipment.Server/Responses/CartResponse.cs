using Newtonsoft.Json;

namespace SalesVentilationEquipment.Server.Responses
{
    [JsonObject]
    public class CartResponse
    {
        #region Fields
        [JsonProperty("id")]
        public required Guid Id { get; set; } = Guid.NewGuid();

        [JsonProperty("totalSum")]
        public required double TotalSum { get; set; }

        [JsonProperty("discount")]
        public required double Discount { get; set; }
        #endregion
    }
}
