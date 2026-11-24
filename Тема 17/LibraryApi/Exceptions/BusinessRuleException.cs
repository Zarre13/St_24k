namespace LibraryApi.Exceptions;

public class BusinessRuleException : AppException
{
    public override int StatusCode => StatusCodes.Status409Conflict;

    public BusinessRuleException(string message) : base(message) { }
}