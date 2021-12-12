using FluentValidation;
using JetBrains.Annotations;

namespace Telegramma.Sender.Api.Sender.SendMessage
{
    [UsedImplicitly]
    public class RequestValidator: AbstractValidator<Request>
    {
        public RequestValidator()
        {
            When(x => string.IsNullOrEmpty(x.RecipientEmail),
                () => RuleFor(x => x.RecipientPhoneNumber).NotEmpty());
            When(x => string.IsNullOrEmpty(x.SenderEmail),
                () => RuleFor(x => x.SenderPhoneNumber).NotEmpty());
            When(x => string.IsNullOrEmpty(x.RecipientPhoneNumber),
                () => RuleFor(x => x.RecipientEmail).NotEmpty());
            When(x => string.IsNullOrEmpty(x.SenderPhoneNumber),
                () => RuleFor(x => x.SenderEmail).NotEmpty());

            RuleFor(x => x.Text).NotEmpty();
        }
    }
}
