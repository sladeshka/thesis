namespace SalesVentilationEquipment.Server.Exceptions
{
    public class UnprocessableEntityException : BaseException
    {
        public override string Message { get; } = "Unprocessable Entity";
        public UnprocessableEntityException()
        {
            Code = StatusCodes.Status422UnprocessableEntity;
        }

        public UnprocessableEntityException(List<ErrorDetail> errors)
        {
            Code = StatusCodes.Status422UnprocessableEntity;
            Errors = errors;
        }

        public UnprocessableEntityException(string error)
        {
            Code = StatusCodes.Status422UnprocessableEntity;
            Message = error;
        }
    }
}
