// TODO: Remove this when the migration is complete.
// using Xunit;
// using Xunit.Abstractions;
//
// using curryware_kafka_producer_library;
//
// namespace curryware_yahoo_api_tests.curryware_kafka_library_tests;
//
// public class ValidateKafkaSettingTest(ITestOutputHelper output)
// {
//     const string KafkaTopic = "PlayerTopic2";
//     
//     [Fact]
//     public void KafkaSettingsTest()
//     {
//         var validateKafkaTopic = ValidateKafkaSettings.ValidateSettings();
//         // These tests are set to always return true because they can't run on GitHub Actions.
//         Assert.True(validateKafkaTopic);
//         Assert.True(true);
//     }
//     
//     [Fact]
//     public async Task KafkaCreateTopicTest()
//     {
//         var topicExists = false;
//         const string topicToCreate = KafkaTopic;
//         var validateKafkaTopic = await KafkaAdmin.CreateTopic(topicToCreate);
//         output.WriteLine(Convert.ToString(validateKafkaTopic));
//         var kafkaTopics = KafkaAdmin.GetTopics();
//         foreach (var topic in kafkaTopics.Where(topic => topic == topicToCreate))
//         {
//             topicExists = true;
//         }
//         // These tests are set to always return true because they can't run on GitHub Actions.
//         Assert.True(topicExists);
//         // Assert.True(true);
//     }
    
//     [Fact]
//     public async Task KafkaProduceToTopicTest()
//     {
//         const string valueJson = """[{"Key":"449.p.41062","Id":41062,"FullName":"Cam Little","Url":"https://sports.yahoo.com/nfl/players/41062","Status":"","Team":"Jax","ByeWeek":12,"UniformNumber":39,"Position":"K","Headshot":"https://s.yimg.com/iu/api/res/1.2/IXs75admNFB6P1upV5Xmvw--~C/YXBwaWQ9eXNwb3J0cztjaD0yMzM2O2NyPTE7Y3c9MTc5MDtkeD04NTc7ZHk9MDtmaT11bGNyb3A7aD02MDtxPTEwMDt3PTQ2/https://s.yimg.com/xe/i/us/sp/v/nfl_cutout/players_l/09062024/41062.png"},{"Key":"449.p.41064","Id":41064,"FullName":"Jawhar Jordan","Url":"https://sports.yahoo.com/nfl/players/41064","Status":"NA","Team":"Hou","ByeWeek":14,"UniformNumber":42,"Position":"RB","Headshot":"https://s.yimg.com/iu/api/res/1.2/TcM85WhJ.fAOHWf2QKLjIw--~C/YXBwaWQ9eXNwb3J0cztjaD0yMDA7Y3I9MTtjdz0xNTM7ZHg9NzQ7ZHk9MDtmaT11bGNyb3A7aD02MDtxPTEwMDt3PTQ2/https://s.yimg.com/dh/ap/default/140828/silhouette@2x.png"}]""";
//         const string topicValue = valueJson;
//         var topicCreated = await KafkaProducer.CreateKafkaMessage(KafkaTopic, topicValue);
//         // These tests are set to always return true because they can't run on GitHub Actions.
//         Assert.True(topicCreated);
//         // Assert.True(true);
//     }
// }