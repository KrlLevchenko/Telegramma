using System;
using System.Threading.Tasks;
using Grpc.Core;
using MediatR;
using Telegramma.Sender.V1;

namespace Telegramma.Sender.Api.Sender
{
    public class SenderV1Controller: SendMessageService.SendMessageServiceBase
    {
        private readonly IMediator _mediator;

        public SenderV1Controller(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override async Task<SendMessageResponse> Send(SendMessageRequest request, ServerCallContext context)
        {
            var req = new SendMessage.Request
            {
                Text = request.Text,
                RecipientEmail = request.RecipientEmail,
                SenderEmail = request.SenderEmail,
                RecipientPhoneNumber = request.RecipientPhoneNumber,
                SenderPhoneNumber = request.SenderPhoneNumber,
            };
            var resp = await _mediator.Send(req, context.CancellationToken);
            return new SendMessageResponse
            {
                MessageId = resp.MessageId.ToString()
            };
        }
    }
}
