using Application.Interfaces.Context;
using Domain.Files;
using Endpoint.Controllers;
using Endpoint.Messages;
using MassTransit;

namespace Endpoint.Consumer
{
    public class SaveToDatabaseConsumer : IConsumer<CreateItemCommand>
    {
        private readonly IDataBaseContext _context;
        private bool _isSaved;

        public SaveToDatabaseConsumer(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<CreateItemCommand> context)
        {
            CsvFiles csv=new CsvFiles{
                Code=context.Message.Item.Code,
                Name=context.Message.Item.Name,
                Value=context.Message.Item.Value,
            };
            _context.csvFiles.Add(csv);
            _isSaved = await _context.SaveChangesAsync() > 0;

            // Send a success event to the next consumer
            await context.Publish(new ItemSavedEvent
            {
                IsSaved = _isSaved,
                Message = $"Item {context.Message.Item.Name} with {context.Message.Item.Code} saved " + _isSaved,
                Item = context.Message.Item,
                At = DateTime.Now
            });
        }

    }
}