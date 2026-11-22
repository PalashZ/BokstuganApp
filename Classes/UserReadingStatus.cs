namespace BokstuganApp.Classes;

public class UserReadingStatus
{
    public string UserName { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public ReadingStatus Status { get; set; } = ReadingStatus.Oläst;
}