using System.Net;
using Grpc.Core;

namespace Telegramma.Sender.Core.ExceptionHandling
{
    public class HandleResult
    {
        public StatusCode StatusCode { get; init; }

        public string[] Errors { get; init; } = new string[0];
    }
}
