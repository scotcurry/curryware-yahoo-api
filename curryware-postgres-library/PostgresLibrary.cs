using System.Text.Json;
using curryware_postgres_library.Models.PlayerModels;
using curryware_postgres_library.PostgresSetup;
using Npgsql;

namespace curryware_postgres_library;

public abstract class PostgresLibrary
{
    public static async Task<string> GetPlayerIdsByPosition(string position = "QB")
    {
        return await PostgresQueryExecutor.ExecuteQueryAsync(async connection =>
        {
            var playerList = new List<PlayerModel>();
            const string selectString = @"
                SELECT player_id, player_season_key, player_name, player_url,
                   player_team, player_bye_week, player_uniform_number, 
                   player_position, player_headshot
              FROM player_info 
             WHERE player_position = @position";

            await using var command = new NpgsqlCommand(selectString, connection);
            command.Parameters.AddWithValue("position", position);

            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var playerId = reader.GetInt32(reader.GetOrdinal("player_id"));
                var playerSeasonKey = reader.GetString(reader.GetOrdinal("player_season_key"));
                var playerName = reader.GetString(reader.GetOrdinal("player_name"));
                var playerUrl = reader.GetString(reader.GetOrdinal("player_url"));
                var playerTeam = reader.GetString(reader.GetOrdinal("player_team"));
                var playerByeWeek = reader.GetInt32(reader.GetOrdinal("player_bye_week"));
                var playerUniformNumber = reader.GetInt32(reader.GetOrdinal("player_uniform_number"));
                var playerPosition = reader.GetString(reader.GetOrdinal("player_position"));
                var playerHeadshot = reader.GetString(reader.GetOrdinal("player_headshot"));

                var playerModel = new PlayerModel()
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

                playerList.Add(playerModel);
            }

            return JsonSerializer.Serialize(playerList);
        });
    }
}
