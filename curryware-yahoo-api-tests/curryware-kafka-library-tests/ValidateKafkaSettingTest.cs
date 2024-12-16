using Xunit;
using Xunit.Abstractions;

using curryware_kafka_producer_library;
    
namespace curryware_yahoo_api_tests.curryware_kafka_library_tests;

public class ValidateKafkaSettingTest(ITestOutputHelper output)
{
    const string kafkaTopic = "CreatedTestTopic";
    
    [Fact]
    public void KafkaSettingsTest()
    {
        var validateKafkaTopic = ValidateKafkaSettings.ValidateSettings();
        Assert.True(validateKafkaTopic);
    }

    [Fact]
    public async Task KafkaCreateTopicTest()
    {
        const string topicToCreate = kafkaTopic;
        var validateKafkaTopic = await KafkaAdmin.CreateTopic(topicToCreate);
        output.WriteLine(Convert.ToString(validateKafkaTopic));
        var kafkaTopics = KafkaAdmin.GetTopics();
        var topicExists = false;
        foreach (var topic in kafkaTopics)
        {
            if (topic == topicToCreate)
                topicExists = true;
        }
        Assert.True(topicExists);
    }

    [Fact]
    public async Task KafkaProduceToTopicTest()
    {
        const string topicValue = "{\"Key\": \"Value\"}";
        var topicCreated = await KafkaProducer.CreateKafkaMessage(kafkaTopic, topicValue);
        Assert.True(topicCreated);
    }
}