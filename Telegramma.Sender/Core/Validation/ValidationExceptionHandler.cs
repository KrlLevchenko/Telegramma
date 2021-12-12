using System;
using System.Linq;
using System.Net;
using FluentValidation;
using Grpc.Core;
using JetBrains.Annotations;
using Telegramma.Sender.Core.ExceptionHandling;

namespace Telegramma.Sender.Core.Validation
{
    [UsedImplicitly]
    public class ValidationExceptionHandler: IExceptionHandler
    {
        public HandleResult? Handle(Exception exception)
        {
            if (exception is ValidationException validationException)
            {
                return new HandleResult
                {
                    Errors = validationException.Errors.Select(e => e.ErrorMessage).ToArray(),
                    StatusCode = StatusCode.InvalidArgument
                };
            }

            return null;
        }
    }
}
