using Confluent.Kafka;
using Serilog;
using Serilog.Formatting.Json;

namespace curryware_yahoo_api.KafkaHandlers;

public static class PlayerProducer
{
     public static async Task<bool> SendPlayerData(string topic, string value)
     {
         Log.Logger = new LoggerConfiguration()
             .WriteTo.Console(new JsonFormatter())
             .CreateLogger();
         
         // Check to see if the environment variable even exists.
         var bootStrapServer = Environment.GetEnvironmentVariable("KAFKA_BOOTSTRAP_SERVER");
         if (bootStrapServer == null)
         {
             return false;
         }
         
         // Check to see if the anything is listening on the Kafka port.
         var kafkaListening = ValidateKafkaSettings.ValidateSettings();
         if (!kafkaListening)
         {
             Log.Error("Kafka Port not open");
             return false;
         }

         Log.Information("Kafka Bootstrap Server: {bootstrapServer}", bootStrapServer);
         // All the values required for this class are documented at (need to research requirements):
         // https://docs.confluent.io/platform/current/clients/confluent-kafka-dotnet/_site/api/Confluent.Kafka.ProducerConfig.html
         var config = new ProducerConfig
         {
             BootstrapServers = bootStrapServer,
             ClientId = "SendPlayerData"
         };

         Guid guid = Guid.NewGuid();
         var key = "key_" + guid;
         var message = new Message<string, string>
         {
             Key = key,
             Value = value
         };
         
         var topicExists = ValidateKafkaSettings.GetValidateTopicExists(topic);
         if (!topicExists)
         {
             Log.Error("Topic does not exist, building topic " + topic);
             var createResult = await KafkaAdmin.CreateTopic(topic);
             Log.Information("Create Result: {createResult}", createResult);
             if (createResult == false)
             {
                 Log.Error("Failed to create topic");
                 return false;
             }
         }
         
         var producer = new ProducerBuilder<string, string>(config).Build();
         var deliveryReport = await producer.ProduceAsync(topic, message);
         Log.Information($"Delivered '{deliveryReport.Value}' to '{deliveryReport.TopicPartitionOffset}'");
         
         producer.Flush(TimeSpan.FromSeconds(10));
         Log.Information("Flushing");
         return true;
     }
}