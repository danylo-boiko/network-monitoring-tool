namespace Nmt.GraphQL.Extensions;

public static class IErrorExtensions
{
    public static IError WithProperty(this IError error, string property)
    {
        return error.WithExtensions(new Dictionary<string, object?>
        {
            { "property", property }
        });
    }
}