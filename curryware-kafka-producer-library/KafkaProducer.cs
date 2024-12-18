using System.Net.Mime;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;

using curryware_log_handler;

namespace curryware_kafka_producer_library;

public abstract class KafkaProducer
{
    public static async Task<bool> CreateKafkaMessage(string topic, string value)
     {
        // Check to see if the environment variable even exists.
         var bootStrapServer = string.Empty;
         try
         {
             if (ValidateKafkaSettings.ValidateSettings())
                bootStrapServer = Environment.GetEnvironmentVariable("KAFKA_BOOTSTRAP_SERVER");
             
            CurrywareLogHandler.AddLog("Kafka Bootstrap Server: " + bootStrapServer, LogLevel.Information);
             // All the values required for this class are documented at (need to research requirements):
             // https://docs.confluent.io/platform/current/clients/confluent-kafka-dotnet/_site/api/Confluent.Kafka.ProducerConfig.html
             var config = new ProducerConfig
             {
                 BootstrapServers = bootStrapServer,
                 ClientId = "SendPlayerData"
             };

             Guid guid = Guid.NewGuid();
             var key = "key_" + guid;
             var valueBytes = System.Text.Encoding.UTF8.GetBytes(value);
             var valueBase64 = Convert.ToBase64String(valueBytes);
             var message = new Message<string, string>
             {
                 Key = key,
                 Value = value
             };

             // var valueScot = "Scot";
             // var textBytes = System.Text.Encoding.UTF8.GetBytes(valueScot);
             // var valueScotBase64 = Convert.ToBase64String(textBytes);
             // var message = new Message<string, string>
             // {
             //     Key = key,
             //     Value = valueScotBase64
             // };
         
             var topicExists = ValidateKafkaSettings.GetValidateTopicExists(topic);
             if (!topicExists)
             {
                 CurrywareLogHandler.AddLog("Topic does not exist, building topic " + topic, LogLevel.Information);
                 var createResult = await KafkaAdmin.CreateTopic(topic);
                 CurrywareLogHandler.AddLog("Create Result: " + createResult, LogLevel.Information);
                 if (createResult == false)
                 {
                     CurrywareLogHandler.AddLog("Failed to create topic", LogLevel.Error);
                     return false;
                 }
             }
         
             var producer = new ProducerBuilder<string, string>(config).Build();
             var deliveryReport = await producer.ProduceAsync(topic, message);
             CurrywareLogHandler.AddLog($"Delivered '{deliveryReport.Value}' to '{deliveryReport.TopicPartitionOffset}'", LogLevel.Information);
         
             producer.Flush(TimeSpan.FromSeconds(10));
             CurrywareLogHandler.AddLog("Flushing", LogLevel.Information);
             return true;
         }
         catch (KafkaValidationException kafkaException)
         {
             CurrywareLogHandler.AddLog(kafkaException.Message, LogLevel.Error);
             throw;
         }
     }
}