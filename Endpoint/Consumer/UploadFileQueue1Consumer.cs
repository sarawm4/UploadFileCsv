using Endpoint.Messages;
using MassTransit;

namespace Endpoint.Consumer
{
    public class UploadFileQueue1Consumer : IConsumer<UploadFileCommand>
    {
        public async Task Consume(ConsumeContext<UploadFileCommand> context)
        {
            var message = context.Message;
            Console.WriteLine($"item {message.Item.Name} proceed");
        }
    }
}