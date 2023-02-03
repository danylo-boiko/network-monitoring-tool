namespace Nmt.Domain.Exceptions;

public class DomainException : Exception
{
    public string Code { get; set; }
    public string Property { get; set; }

    public DomainException(string message): base(message)
    {
    }
}