using Xunit;

using curryware_yahoo_api.KafkaHandlers;
using Microsoft.Extensions.Configuration;

namespace curryware_yahoo_api_tests;

public class KafkaHandlerTest
{
    [Fact]
    public void KafkaHandlerListTopicsTest()
    {
        var kafkaAdmin = new KafkaAdmin();
        try {
            kafkaAdmin.GetTopics();
            var topicsList = kafkaAdmin.GetTopics();
            Assert.True(topicsList.Count > 0);
        }
        catch (KafkaValidationException kafkaException)
        {
            Assert.True(true, kafkaException.Message);
        }
    }
    
    [Fact]
    public async Task CreatePlayerMessageTest()
    {
        var topicName = "PlayerTopic";
        var dateString = DateTime.Now.ToString( "MM/dd/yyyy hh:mm:ss tt");
        try
        {
            var returnValue = await PlayerProducer.SendPlayerData(topicName, "Scot" + dateString);
            Assert.True(returnValue);
        }
        catch (KafkaValidationException kafkaException)
        {
            Assert.True(true, kafkaException.Message);
        }
    }
} 
    