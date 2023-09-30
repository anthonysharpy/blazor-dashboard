namespace blazor_dashboard;

public class Server
{
    public long SteamID { get; set; }
    public string ServerName { get; set; }
    public string CurrentGame { get; set; }
    public int Ping { get; set; }
    public int MaxPlayers { get; set; }
    public DateTime StartTime { get; set; }
    public List<Player> Players { get; set; }

    public TimeSpan GetUptime()
    {
        return DateTime.Now - StartTime;
    }

    public Server Clone()
    {
        return new Server()
        {
            SteamID = SteamID,
            ServerName = ServerName,
            CurrentGame = CurrentGame,
            StartTime = StartTime,
            Players = Players.Select(x => x.Clone()).ToList(),
            Ping = Ping,
            MaxPlayers = MaxPlayers
        };
    }
}

