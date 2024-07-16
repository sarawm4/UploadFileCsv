using Domain.Files;

namespace Endpoint.Messages;

public class CreateItemCommand
{
    public CsvFiles Item { get; set; }
    public DateTime At { get; set; }
}