namespace CreativeCoders.IO.Archives;

public class TarFileOwnerInfo
{
    public int UserId { get; init; }

    public int GroupId { get; init; }

    public string UserName { get; init; } = string.Empty;

    public string GroupName { get; init; } = string.Empty;
}
