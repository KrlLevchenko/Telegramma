using System;

namespace Telegramma.KafkaContracts
{
    public class MessageDto
    {
        public const string TopicName = "messages";

        public Guid MessageId { get; set; }
        public string? RecipientEmail { get; init; }
        public string? SenderEmail { get; init; }
        public string? RecipientPhoneNumber { get; init; }
        public string? SenderPhoneNumber { get; init; }
    }
}
