using Domain.Attributes;

namespace Domain.Files;

[Auditable]
public class CsvFiles
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
}

