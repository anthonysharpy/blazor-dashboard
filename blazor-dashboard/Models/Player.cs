namespace blazor_dashboard;

public class Player
{
    public string SteamName { get; set; }
    public long SteamID { get; set; }
    public bool IsBanned { get; set; }
    public TimeSpan PlayTime { get; set; }
    public string LastKnownIPv4 { get; set; }

    public Player Clone()
    {
        return new Player
        {
            SteamName = SteamName,
            SteamID = SteamID,
            IsBanned = IsBanned,
            PlayTime = PlayTime,
            LastKnownIPv4 = LastKnownIPv4
        };
    }
}

