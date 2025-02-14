using System.Text.Json;
using curryware_data_models;
using curryware_fantasy_command_line_tool.KafkaCommands;
using curryware_log_handler;
using Microsoft.Extensions.Logging;

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
            CurrywareLogHandler.AddLog(jsonException.Message, LogLevel.Error);
        }
        catch (NotSupportedException notSupportedException)
        {
            CurrywareLogHandler.AddLog(notSupportedException.Message, LogLevel.Error);
        }

        if (statsJson?.PlayerStats == null) return success;
        var statsToStore = JsonSerializer.Serialize(statsJson.PlayerStats);
        success = await ProducePlayerStats.StorePlayerStats(statsToStore);
        return success;
    }
}