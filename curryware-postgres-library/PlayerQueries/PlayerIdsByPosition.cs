using System.Text.Json;
using curryware_data_models;
using Npgsql;

namespace curryware_postgres_library.PlayerQueries;

internal abstract class PlayerInfoByPosition
{
    internal static async Task<string> PlayerQuery(NpgsqlDataSource dataSource, string position)
    {
        var playerList = new List<PlayerModel>();
        const string selectString = "SELECT player_id, player_season_key FROM players_info WHERE player_position = '@player_position'";

        try
        {
            await using var command = dataSource.CreateCommand(selectString);
            command.Parameters.AddWithValue("player_position", position);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var playerId = reader.GetInt32(0);
                var playerSeasonKey = reader.GetString(1);
                var playerName = reader.GetString(2);
                var playerUrl = reader.GetString(3);
                var playerTeam = reader.GetString(4);
                var playerByeWeek = reader.GetInt32(5);
                var playerUniformNumber = reader.GetInt32(6);
                var playerPosition = reader.GetString(7);
                var playerHeadshot = reader.GetString(8);

                var playerInfo = new PlayerModel
                {
                    Id = playerId,
                    Key = playerSeasonKey,
                    FullName = playerName,
                    Url = playerUrl,
                    Team = playerTeam,
                    ByeWeek = playerByeWeek,
                    UniformNumber = playerUniformNumber,
                    Position = playerPosition,
                    Headshot = playerHeadshot
                };
                playerList.Add(playerInfo);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return JsonSerializer.Serialize(playerList);
    }
}