using System.Net.Sockets;
using curryware_yahoo_api.LogHandler;

namespace curryware_yahoo_api.KafkaHandlers;

public static class ValidateKafkaSettings
{
    public static bool ValidateSettings()
    {
        CurrywareLogHandler.AddLog("Validating Kafka Port", LogLevel.Information);
        
        var bootStrapServer = Environment.GetEnvironmentVariable("KAFKA_BOOTSTRAP_SERVER");
        if (bootStrapServer == null)
            throw new KafkaValidationException("No Kafka Bootstrap Server Environment Variable Set!");
        
        var partsToValidate = bootStrapServer.Split(":");
        var host = partsToValidate[0];
        var portString = partsToValidate[1];
        int port = Convert.ToInt32(portString);
        CurrywareLogHandler.AddLog($"Kafka Host: {host}, Kafka Port: {port}", LogLevel.Information);
        
        try
        {
            using var client = new TcpClient();
            var testResult = client.BeginConnect(host, port, null, null);
            var success = testResult.AsyncWaitHandle.WaitOne(1000);

            if (!success)
            {
                CurrywareLogHandler.AddLog("Kafka Port not open", LogLevel.Error);
                throw new KafkaValidationException("Kafka Port Not Listening");
            }

            client.EndConnect(testResult);
            return true;
        }
        catch (ArgumentNullException argumentNull)
        {
            CurrywareLogHandler.AddLog(argumentNull.Message, LogLevel.Error);
            throw new KafkaValidationException("Argument null!", argumentNull);
        }
        catch (SocketException socketException)
        {
            CurrywareLogHandler.AddLog(socketException.Message, LogLevel.Error);
            throw new KafkaValidationException("Socket Exception!", socketException);
        }
        catch (ArgumentException argumentException)
        {
            CurrywareLogHandler.AddLog(argumentException.Message, LogLevel.Error);
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
            if (currentTopic != topicName)
            {
                topicExists = true;
                i = allTopics.Count;
                CurrywareLogHandler.AddLog("Kafka Port Exists", LogLevel.Information);
            }
            else
            {
                CurrywareLogHandler.AddLog("Kafka Port Doesn't Exists", LogLevel.Error);
            }
        } 
        
        return topicExists;
    }
}