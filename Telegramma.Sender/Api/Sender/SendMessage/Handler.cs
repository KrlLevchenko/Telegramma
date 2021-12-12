using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using JetBrains.Annotations;
using MediatR;
using Telegramma.KafkaContracts;

namespace Telegramma.Sender.Api.Sender.SendMessage
{
    [UsedImplicitly]
    public class Handler: IRequestHandler<Request, Response>
    {
        private readonly ProducerConfig _producerConfig;

        public Handler(ProducerConfig producerConfig)
        {
            _producerConfig = producerConfig;
        }

        public async Task<Response> Handle(Request request, CancellationToken ct)
        {
            var message = new MessageDto
            {
                MessageId = Guid.NewGuid(),
                RecipientEmail = request.RecipientEmail,
                SenderEmail = request.SenderEmail,
                RecipientPhoneNumber = request.RecipientPhoneNumber,
                SenderPhoneNumber = request.SenderPhoneNumber,
            };

            await SendMessageToKafka(message, ct);
            return new Response
            {
                MessageId = message.MessageId
            };
        }

        private async Task SendMessageToKafka(MessageDto message, CancellationToken ct)
        {
            var producerBuilder = new ProducerBuilder<Null, string>(_producerConfig);
            using var producer = producerBuilder.Build();
            await producer.ProduceAsync(MessageDto.TopicName, new Message<Null, string>
            {
                Value = JsonSerializer.Serialize(message)
            }, ct);
            producer.Flush(ct);
        }

    }




}
