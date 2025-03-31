using Confluent.Kafka;
using Serilog;

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
             
            //CurrywareLogHandler.AddLog("Kafka Bootstrap Server: " + bootStrapServer, LogLevel.Information);
            Log.Debug($"Kafka Bootstrap Server: {bootStrapServer}");
             // All the values required for this class are documented at (need to research requirements):
             // https://docs.confluent.io/platform/current/clients/confluent-kafka-dotnet/_site/api/Confluent.Kafka.ProducerConfig.html
             var config = new ProducerConfig
             {
                 BootstrapServers = bootStrapServer,
                 ClientId = "SendPlayerData"
             };

             var guid = Guid.NewGuid();
             var key = "key_" + guid;
             var valueBytes = System.Text.Encoding.UTF8.GetBytes(value);
             var valueBase64 = Convert.ToBase64String(valueBytes);
             var message = new Message<string, string>
             {
                 Key = key,
                 Value = valueBase64
             };
             
             var topicExists = ValidateKafkaSettings.GetValidateTopicExists(topic);
             if (!topicExists)
             {
                 //CurrywareLogHandler.AddLog("Topic does not exist, building topic " + topic, LogLevel.Information);
                 Log.Debug("Topic does not exist, building topic {topic}", topic);
                 var createResult = await KafkaAdmin.CreateTopic(topic);
                 // CurrywareLogHandler.AddLog("Create Result: " + createResult, LogLevel.Information);
                 Log.Debug("Create Result: {createResult}", createResult);
                 if (createResult == false)
                 {
                     // CurrywareLogHandler.AddLog("Failed to create topic", LogLevel.Error);
                     Log.Error("Failed to create topic {topic}", topic);
                     return false;
                 }
             }
         
             var producer = new ProducerBuilder<string, string>(config).Build();
             var deliveryReport = await producer.ProduceAsync(topic, message);
             // CurrywareLogHandler.AddLog($"Delivered '{deliveryReport.Value}' to '{deliveryReport.TopicPartitionOffset}'", LogLevel.Information);
             Log.Debug($"Delivered '{deliveryReport.Value.AsSpan(1,40)}' to '{deliveryReport.TopicPartitionOffset}'");
         
             producer.Flush(TimeSpan.FromSeconds(10));
             // CurrywareLogHandler.AddLog("Flushing", LogLevel.Information);
             Log.Debug("Flushing");
             return true;
         }
         catch (KafkaValidationException kafkaException)
         {
             // CurrywareLogHandler.AddLog(kafkaException.Message, LogLevel.Error);
             Log.Error(kafkaException.Message);
             throw;
         }
     }
}