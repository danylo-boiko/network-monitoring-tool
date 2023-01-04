namespace Nmt.GraphQL;

public class ErrorFilter : IErrorFilter
{
    public IError OnError(IError error)
    {
        return error.WithMessage(error.Exception != null ? error.Exception.Message : error.Message);
    }
}