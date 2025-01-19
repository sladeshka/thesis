namespace SalesVentilationEquipment.Server.Exceptions
{
    public class ForbiddenException : BaseException
    {
        public override string Message { get; } = "Forbidden";
        public ForbiddenException()
        {
            Code = StatusCodes.Status403Forbidden;
        }
    }
}
