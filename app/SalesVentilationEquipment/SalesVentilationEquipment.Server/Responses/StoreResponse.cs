using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SalesVentilationEquipment.Server.Responses
{
    public class StoreResponse
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
