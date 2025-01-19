using Newtonsoft.Json;

namespace SalesVentilationEquipment.Server.Responses
{
    [JsonObject]
    public class OkResponse
    {
        #region Fields
        [JsonProperty("code")]
        public int Code { get; set; } = 200;

        [JsonProperty("data")]
        public object Data { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; } = "OK";
        #endregion
    }
}
