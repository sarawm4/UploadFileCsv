using Domain.Files;

namespace Endpoint.Messages
{
    public class UploadFileCommand
    {
        public CsvFiles Item { get; set; }
        public DateTime At { get; set; }
    }
}
