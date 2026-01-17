namespace CreativeCoders.IO.Archives.Tar;

public class TarEntryInfo
{
    public string UserName { get; set; } = string.Empty;

    public int UserId { get; set; }

    public string GroupName { get; set; } = string.Empty;

    public int GroupId { get; set; }

    public UnixFileMode FileMode { get; set; }

    public IReadOnlyDictionary<string, string> ExtendedAttributes { get; init; } =
        new Dictionary<string, string>();
}
