using Domain.Files;

namespace Endpoint.Messages;

public class ProcessItemCommand
{
    public CsvFiles Item { get; set; }
    public DateTime At { get; set; }
}