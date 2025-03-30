using System.Text.Json;
using curryware_data_models;
using curryware_postgres_library;
using Serilog;

namespace curryware_fantasy_command_line_tool.StatsCommands;

public abstract class GetPlayersFromPostgres
{
    // This returns a list of PlayerModels
    public static async Task<List<string>> GetPlayersByPosition(string position)
    {
        var playerListJson = await PostgresLibrary.GetPlayerIdsByPosition(position);
        var players= GetPlayersFromJson(playerListJson);
        if (players.Count == 0)
        {
            // CurrywareLogHandler.AddLog("No players found for position.", LogLevel.Error);
            Log.Error("no players found for position");
            return [];
        }
        
        return players;
    }

    private static List<string> GetPlayersFromJson(string json)
    {
        var allPlayersList = new List<string>();
        try
        {
            var allPlayers = JsonSerializer.Deserialize<List<PlayerModel>>(json);
            if (allPlayers != null)
            {
                foreach (var player in allPlayers)
                {
                    allPlayersList.Add(Convert.ToString(player.Id));
                }
                return allPlayersList;
            }

            // CurrywareLogHandler.AddLog("No players found.", LogLevel.Error);
            Log.Error("No players found.");
            return [];
        }
        catch (ArgumentNullException argumentNullException)
        {
            // CurrywareLogHandler.AddLog(argumentNullException.Message, LogLevel.Error);
            Log.Error(argumentNullException.Message);
            return [];
        }
        catch (JsonException jsonException)
        {
            // CurrywareLogHandler.AddLog(jsonException.Message, LogLevel.Error);
            Log.Error(jsonException.Message);
            return [];
        }
    }
}