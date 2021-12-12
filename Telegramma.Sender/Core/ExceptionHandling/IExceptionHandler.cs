using System;

namespace Telegramma.Sender.Core.ExceptionHandling
{
    public interface IExceptionHandler
    {
        HandleResult? Handle(Exception exception);
    }
}
