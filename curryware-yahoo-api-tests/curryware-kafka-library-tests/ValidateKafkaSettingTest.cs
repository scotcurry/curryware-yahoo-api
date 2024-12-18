// using Xunit;
// using Xunit.Abstractions;
//
// using curryware_kafka_producer_library;

// namespace curryware_yahoo_api_tests.curryware_kafka_library_tests;
//
// public class ValidateKafkaSettingTest(ITestOutputHelper output)
// {
    // const string KafkaTopic = "CreatedTestTopic";
    //
    // [Fact]
    // public void KafkaSettingsTest()
    // {
    //     var validateKafkaTopic = ValidateKafkaSettings.ValidateSettings();
    //     // These tests are set to always return true because they can't run on GitHub Actions.
    //     Assert.True(validateKafkaTopic);
    //     //Assert.True(true);
    // }
    //
    // [Fact]
    // public async Task KafkaCreateTopicTest()
    // {
    //     var topicExists = false;
    //     const string topicToCreate = KafkaTopic;
    //     var validateKafkaTopic = await KafkaAdmin.CreateTopic(topicToCreate);
    //     output.WriteLine(Convert.ToString(validateKafkaTopic));
    //     var kafkaTopics = KafkaAdmin.GetTopics();
    //     foreach (var topic in kafkaTopics.Where(topic => topic == topicToCreate))
    //     {
    //         topicExists = true;
    //     }
    //     // These tests are set to always return true because they can't run on GitHub Actions.
    //     Assert.True(topicExists);
    //     // Assert.True(true);
    // }
    //
    // [Fact]
    // public async Task KafkaProduceToTopicTest()
    // {
    //     const string topicValue = "{\"Key\": \"Value\"}";
    //     var topicCreated = await KafkaProducer.CreateKafkaMessage(KafkaTopic, topicValue);
    //     // These tests are set to always return true because they can't run on GitHub Actions.
    //     Assert.True(topicCreated);
    //     // Assert.True(true);
    // }
// }