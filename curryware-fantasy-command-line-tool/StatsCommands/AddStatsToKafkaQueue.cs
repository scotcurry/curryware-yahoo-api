using System.Text.Json;
using curryware_data_models;
using curryware_fantasy_command_line_tool.KafkaCommands;
using Serilog;

namespace curryware_fantasy_command_line_tool.StatsCommands;

public abstract class AddStatsToKafkaQueue
{
    public static async Task<bool> AddStatsToQueue(string statsLine)
    {
        var statsJson = new PlayerStatsWithMetadata();
        var success = false;
        try
        {
            statsJson = JsonSerializer.Deserialize<PlayerStatsWithMetadata>(statsLine);
        }
        catch (JsonException jsonException)
        {
            // CurrywareLogHandler.AddLog(jsonException.Message, LogLevel.Error);
            Log.Error(jsonException.Message);
        }
        catch (NotSupportedException notSupportedException)
        {
            // CurrywareLogHandler.AddLog(notSupportedException.Message, LogLevel.Error);
            Log.Error(notSupportedException.Message);
        }

        if (statsJson?.PlayerStats == null) return success;
        var statsToStore = JsonSerializer.Serialize(statsJson.PlayerStats);
        success = await ProducePlayerStats.StorePlayerStats(statsToStore);
        return success;
    }
}