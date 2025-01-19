using Newtonsoft.Json;
using SalesVentilationEquipment.Server.Exceptions;

namespace SalesVentilationEquipment.Server.Responses
{
    [JsonObject]
    public class ErrorResponse<T> where T : BaseException
    {
        #region Fields
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("errors")]
        public List<ErrorDetail> Errors { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
        #endregion

        #region Constructor
        public ErrorResponse(T exception)
        {
            Code = exception.Code;
            Errors = exception.Errors;
            Message = exception.Message;
        }
        #endregion
    }
}
