using FluentValidation;
using Nmt.Domain.Exceptions;
using Nmt.GraphQL.Extensions;

namespace Nmt.GraphQL.Filters;

public class ErrorFilter : IErrorFilter
{
    public IError OnError(IError error)
    {
        if (error.Exception == null)
        {
            return error.WithMessage(error.Message);
        }

        switch (error.Exception)
        {
            case ValidationException validationException:
                return new AggregateError(validationException.Errors.Select(e => new Error(e.ErrorMessage)
                    .WithCode(e.ErrorCode)
                    .WithProperty(e.PropertyName)));
            case DomainException domainException:
                return error
                    .WithMessage(domainException.Message)
                    .WithCode(domainException.Code)
                    .WithProperty(domainException.Property);
        }

        return error.WithMessage(error.Exception.Message);
    }
}