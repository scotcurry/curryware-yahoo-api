using Xunit;

using curryware_yahoo_api.KafkaHandlers;

namespace curryware_yahoo_api_tests;

public class KafkaHandlerTest
{
    [Fact]
    public void KafkaHandlerListTopicsTest()
    {
        var kafkaAdmin = new KafkaAdmin();
        var topicsList = kafkaAdmin.GetTopics();
        Assert.True(topicsList.Count > 0);
    }

    [Fact]
    public async Task KafkaCreateTopicTest()
    {
        var topicName = "PlayerInfo";
        var kafkaAdmin = new KafkaAdmin();
        await kafkaAdmin.CreateTopic(topicName);
    }

    [Fact]
    public async Task CreatePlayerMessageTest()
    {
        var topicName = "PlayerTopic";
        var returnValue = await PlayerProducer.SendPlayerData(topicName, "Scot");
        Assert.True(returnValue);
    }
} 
    