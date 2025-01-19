using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SalesVentilationEquipment.Server.Requests
{
    public class ContractorRequest
    {
        #region Fields
        [JsonProperty("id")]
        public required Guid Id { get; set; }

        [JsonProperty("name", Required = Required.Always)]
        [StringLength(150)]
        public required string Name { get; set; }

        [JsonProperty("contact_info", Required = Required.Always)]
        [StringLength(150)]
        public required string ContactInfo { get; set; }
        #endregion
    }
}
