using System.Text.Json;
using curryware_yahoo_parsing_library.PlayerApis;
using curryware_data_models;
using curryware_fantasy_command_line_tool.CommandLineModels;
using curryware_kafka_producer_library;
using Serilog;

namespace curryware_fantasy_command_line_tool.PlayerCommands;

public abstract class PlayerCommand
{
    public static async Task<int> RunGetPlayersCommand(PlayerCommandLineParameters playerCommandLineParameters)
    {
        const string kafkaTopic = "PlayerTopic2";
        var gameId = playerCommandLineParameters.GameId;
        var leagueId = playerCommandLineParameters.LeagueId;
        var playerPosition = playerCommandLineParameters.PlayerPosition;
        var playerStatus = playerCommandLineParameters.PlayerStatus;
        var startNumber = 0;
        var totalBatches = 0;
        var morePlayers = true;
        var oauthToken = "NoToken";

        //CurrywareLogHandler.AddLog(
        //    "Call GetAllPlayersApi.GetAllPlayers with status: " + playerStatus + " and position: " + playerPosition,
        //    LogLevel.Debug);
        Log.Debug("Call GetAllPlayersApi.GetAllPlayers with status: " + playerStatus + " and position: " + playerPosition);
        while (morePlayers)
        {
            var playerJson = string.Empty;
            if (playerPosition != "None" && playerStatus != "None")
                playerJson = await GetAllPlayersApi.GetAllPlayers(oauthToken, gameId, leagueId, startNumber, playerStatus!,
                    playerPosition!);
            if (playerPosition != "None" && playerStatus == "None")
                playerJson =
                    await GetAllPlayersApi.GetAllPlayers(oauthToken, gameId, leagueId, startNumber, status: playerStatus,
                        position: playerPosition!);
            if (playerPosition == "None" && playerStatus != "None")
                playerJson = await GetAllPlayersApi.GetAllPlayers(oauthToken, gameId, leagueId, startNumber, playerStatus!);
            if (playerPosition == "None" && playerStatus == "None")
                playerJson = await GetAllPlayersApi.GetAllPlayers(oauthToken, gameId, leagueId, startNumber);

            if (playerJson == null) continue;
            // CurrywareLogHandler.AddLog("PlayerJson: " + playerJson.Substring(20) + "...", LogLevel.Debug);
            Log.Debug(string.Concat("PlayerJson: ", playerJson.AsSpan(20), "..."));
            var playersModel = JsonSerializer.Deserialize<PlayersListWithCount>(playerJson);
            if (playersModel == null) continue;
            if (playersModel.Players?.Count < 25)
                morePlayers = false;
            startNumber += 25;
            if (playersModel.OAuthToken == null) continue;
            oauthToken = playersModel.OAuthToken;

            try
            {
                Log.Debug($"Writing {playersModel.Players?.Count} to JSON ");
                var justPlayers = JsonSerializer.Serialize(playersModel.Players);
                var kafkaResult = await KafkaProducer.CreateKafkaMessage(kafkaTopic, justPlayers);
                if (kafkaResult)
                    totalBatches++;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        return totalBatches;
    }
}