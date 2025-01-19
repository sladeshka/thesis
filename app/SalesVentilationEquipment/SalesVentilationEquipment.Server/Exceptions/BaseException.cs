namespace SalesVentilationEquipment.Server.Exceptions
{
    public abstract class BaseException : Exception
    {
        public int Code { get; set; }
        public List<ErrorDetail>? Errors { get; set; }
        public abstract override string Message { get; }
    }

    public class ErrorDetail
    {
        public required string Field { get; set; }
        public required string Message { get; set; }
    }
}
