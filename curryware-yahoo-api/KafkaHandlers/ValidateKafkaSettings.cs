using System.Net.Sockets;
using Serilog;
using Serilog.Formatting.Json;


namespace curryware_yahoo_api.KafkaHandlers;

public class ValidateKafkaSettings
{
    public static bool ValidateSettings()
    {
        
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(new JsonFormatter())
            .CreateLogger();
        Log.Information("Validating Kafka Port");
        
        var bootStrapServer = Environment.GetEnvironmentVariable("KAFKA_BOOTSTRAP_SERVER");
        if (bootStrapServer == null)
            throw new KafkaValidationException("No Kafka Bootstrap Server Environment Variable Set!");
        
        var partsToValidate = bootStrapServer.Split(":");
        var host = partsToValidate[0];
        var portString = partsToValidate[1];
        int port = Convert.ToInt32(portString);
        
        try
        {
            using var client = new TcpClient();
            var testResult = client.BeginConnect(host, port, null, null);
            var success = testResult.AsyncWaitHandle.WaitOne(1000);

            if (!success)
            {
                Log.Error("Kafka Port not open");
                throw new KafkaValidationException("Kafka Port Not Listening");
            }

            client.EndConnect(testResult);
            return true;
        }
        catch (ArgumentNullException argumentNull)
        {
            Log.Error(argumentNull.Message);
            throw new KafkaValidationException("Argument null!", argumentNull);
        }
        catch (SocketException socketException)
        {
            Log.Error(socketException.Message);
            throw new KafkaValidationException("Socket Exception!", socketException);
        }
        catch (ArgumentException argumentException)
        {
            Log.Error(argumentException.Message);
            throw new KafkaValidationException("Argument Exception!", argumentException);
        }
    }
    
    internal static bool GetValidateTopicExists(string topicName)
    {
        var kafkaAdmin = new KafkaAdmin();
        var allTopics = kafkaAdmin.GetTopics();
        var topicExists = false;
        for (int i = 0; i < allTopics.Count; i++)
        {
            var currentTopic = allTopics[i];
            if (currentTopic != topicName) continue;
            topicExists = true;
            i = allTopics.Count;
        }
        return topicExists;
    }
}