using Endpoint.Messages;
using MassTransit;

namespace Endpoint.Consumer
{
    public class ProcessUploadedItemConsumer : IConsumer<ProcessItemCommand>
    {
        public async Task Consume(ConsumeContext<ProcessItemCommand> context)
        {
           
            var message = context.Message;
            Console.WriteLine($"item {message.Item.Name} proceed");
            // Forward each item to the next consumer
            await context.Publish(
                new CreateItemCommand
                {
                    Item = message.Item,
                    At = DateTime.Now
                });
        }
    }
}