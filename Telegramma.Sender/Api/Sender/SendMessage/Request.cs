using System;
using MediatR;

namespace Telegramma.Sender.Api.Sender.SendMessage
{
    public class Request: IRequest<Response>
    {
        public string Text { get; init; } = "";
        public string? RecipientEmail { get; init; }
        public string? SenderEmail { get; init; }
        public string? RecipientPhoneNumber { get; init; }
        public string? SenderPhoneNumber { get; init; }
    }
}
