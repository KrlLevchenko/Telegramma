using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;

namespace Telegramma.Sender.Api.Sender.SendMessage
{
    [UsedImplicitly]
    public class Handler: IRequestHandler<Request, Response>
    {
        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return new Response
            {
                MessageId = Guid.NewGuid()
            };
        }
    }



}
