using Microsoft.AspNetCore.Hosting.Server;

namespace blazor_dashboard;

/// <summary>
/// This is definitely not how you would actually set up the database!
/// This is just for pretend, I thought it would be a bit overkill to add
/// a full database implementation here :-)
/// 
/// My personal preference would be to go with Dapper because of its speed and
/// lesser chances of shooting yourself in the foot, but Entity Framework is
/// probably more common so you could just as well use that too.
/// </summary>
public class PretendDatabase
{
    private static List<Player> _players = new()
    {
        new Player{LastKnownIPv4 = "44.18.34.112", PlayTime = new(40, 0, 0), SteamID = 12345678, SteamName = "Bob"},
        new Player{LastKnownIPv4 = "44.18.34.113", PlayTime = new(50, 0, 0), SteamID = 12345679, SteamName = "Steve"},
        new Player{LastKnownIPv4 = "44.18.34.114", PlayTime = new(10, 0, 0), SteamID = 12345680, SteamName = "Derek"},
        new Player{LastKnownIPv4 = "44.18.34.115", PlayTime = new(80, 0, 0), SteamID = 12345681, SteamName = "Susan"},
        new Player{LastKnownIPv4 = "44.18.34.116", PlayTime = new(400, 0, 0), SteamID = 12345682, SteamName = "Charlie"},
        new Player{LastKnownIPv4 = "44.18.34.117", PlayTime = new(30, 0, 0), SteamID = 12345683, SteamName = "Charlotte"},
        new Player{LastKnownIPv4 = "44.18.34.118", PlayTime = new(4, 0, 0), SteamID = 12345684, SteamName = "Sarah"},
        new Player{LastKnownIPv4 = "44.18.34.119", PlayTime = new(0, 32, 0), SteamID = 12345685, SteamName = "Fredrick"},
        new Player{LastKnownIPv4 = "44.18.34.110", PlayTime = new(8, 0, 0), SteamID = 12345686, SteamName = "Carl"}
    };

    private static List<Server> _servers = new()
    {
        new Server
        {
            ServerName = "Cool kidz server",
            SteamID = 1251675335222,
            CurrentGame = "Call Of Doody",
            Ping = 84,
            MaxPlayers = 24,
            StartTime = DateTime.Now.AddMinutes(-30),
            Players = new(){ _players[0], _players[1], _players[2], _players[3] }
        },
        new Server
        {
            ServerName = "OMG DONT JOIN!!",
            SteamID = 1253235335454,
            CurrentGame = "Burp Heroes",
            Ping = 122,
            MaxPlayers = 1,
            StartTime = DateTime.Now.AddMinutes(-85),
            Players = new(){ _players[4] }
        },
        new Server
        {
            ServerName = "[OFFICIAL] Gamer Gang",
            SteamID = 1253521798774,
            CurrentGame = "Gamer Chair Simulator",
            Ping = 21,
            MaxPlayers = 8,
            StartTime = DateTime.Now.AddMinutes(-622),
            Players = new(){ _players[5], _players[6], _players[7], _players[8] }
        },
    };

    public static void UpsertServer(Server server)
    {
        _servers = _servers.Where(x => x.SteamID != server.SteamID).ToList();

        // Clone everything going in and out of this fake database, because if
        // we pass references and they get edited, this could cause some weird
        // behaviour. Obviously, this wouldn't be the case with a real database.
        _servers.Add(server.Clone());
    }

    public static void UpsertPlayer(Player player)
    {
        var clonedPlayer = player.Clone();

		_players = _players.Where(x => x.SteamID != player.SteamID).ToList();
		_players.Add(clonedPlayer);

        // Also need to update the players in the server objects. This would
        // never happen with a real database, it's just because we are being
        // awkard and faking it.
        var serverPlayerIsIn = _servers.SingleOrDefault(x => x.Players.Any(p => p.SteamID == player.SteamID));

        if (serverPlayerIsIn != null) 
        {
            serverPlayerIsIn.Players = serverPlayerIsIn.Players.Where(x => x.SteamID != player.SteamID).ToList();
            serverPlayerIsIn.Players.Add(clonedPlayer);
        }
    }

    public static List<Server> GetServers()
    {
        return _servers.Select(x => x.Clone()).ToList();
    }

    public static List<Player> GetPlayers()
    {
        return _players.Select(x => x.Clone()).ToList();
    }
}
