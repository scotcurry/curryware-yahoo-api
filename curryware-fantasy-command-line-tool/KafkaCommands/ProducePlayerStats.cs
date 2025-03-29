using curryware_kafka_producer_library;
using Serilog;

namespace curryware_fantasy_command_line_tool.KafkaCommands;

internal abstract class ProducePlayerStats
{
    internal static async Task<bool> StorePlayerStats(string playerStatsJson)
    {
        const string topic = "PlayerStats";

        try
        {
            var kafkaProducerReturnValue = await KafkaProducer.CreateKafkaMessage(topic, playerStatsJson);
            return kafkaProducerReturnValue;
        }
        catch (KafkaValidationException kafkaValidationException)
        {
            // CurrywareLogHandler.AddLog(kafkaValidationException.Message, LogLevel.Error);
            Log.Error(kafkaValidationException.Message);
            return false;
        }
    }
}