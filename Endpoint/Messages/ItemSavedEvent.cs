using Domain.Files;
using Endpoint.Model;

namespace Endpoint.Messages;

public class ItemSavedEvent
{
    public bool IsSaved { get; set; }
    public string Message { get; set; }
    public CsvFiles Item { get; set; }
    public DateTime At { get; set; }
}