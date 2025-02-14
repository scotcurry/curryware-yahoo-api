using System.Text.Json;
using curryware_data_models;
using curryware_kafka_producer_library;
using curryware_log_handler;
using Microsoft.Extensions.Logging;

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
            CurrywareLogHandler.AddLog(kafkaValidationException.Message, LogLevel.Error);
            return false;
        }
    }
}