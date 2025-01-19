using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SalesVentilationEquipment.Server.Requests
{
    [JsonObject]
    public class ProductRequest
    {
        #region Fields
        [JsonProperty("id")]
        public required Guid Id { get; set; }

        [JsonProperty("name")]
        [StringLength(50)]
        public required string Name { get; set; }

        [JsonProperty("price")]
        public required double Price { get; set; }

        [JsonProperty("description")]
        [StringLength(100)]
        public string? Description { get; set; }

        [JsonProperty("feature")]
        [StringLength(100)]
        public string? Feature { get; set; }
        #endregion
    }
}
