using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SalesVentilationEquipment.Server.Requests
{
    public class WarehouseRequest
    {
        #region Fields
        [JsonProperty("id")]
        public required Guid Id { get; set; }

        [JsonProperty("name", Required = Required.Always)]
        [StringLength(150)]
        public required string Name { get; set; }

        [JsonProperty("address", Required = Required.Always)]
        [StringLength(150)]
        public required string Address { get; set; }
        #endregion
    }
}
