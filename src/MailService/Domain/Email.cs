namespace Domain;

public class EmailTemplate
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Header { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}
