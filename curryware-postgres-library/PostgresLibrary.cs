using System.Text.Json;
using curryware_postgres_library.Models.PlayerModels;
using Npgsql;

namespace curryware_postgres_library;

public abstract class PostgresLibrary
{
    public static async Task<string> GetPlayerIdsByPosition(string position = "QB")
    {
        var postgresUserName = Environment.GetEnvironmentVariable("POSTGRES_USERNAME");
        var postgresPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
        var postgresServer = Environment.GetEnvironmentVariable("POSTGRES_SERVER");
        var postgresPort = Environment.GetEnvironmentVariable("POSTGRES_PORT");
        var postgresDatabase = Environment.GetEnvironmentVariable("POSTGRES_DATABASE");
        
        var playerList = new List<PlayerModel>();
        try
        {
            var connectionStringBuilder = new NpgsqlConnectionStringBuilder
            {
                Host = postgresServer,
                Port = Convert.ToInt32(postgresPort),
                Username = postgresUserName,
                Password = postgresPassword,
                Database = postgresDatabase
            };
            var connectionString = connectionStringBuilder.ConnectionString;
            
            await using var dataSource = NpgsqlDataSource.Create(connectionString); 
            await using (var conn = await dataSource.OpenConnectionAsync())
            {
                var command = conn.CreateCommand();
                var commandText = "SELECT player_id, player_season_key, player_name, player_url, ";
                commandText += "player_team, player_bye_week, player_uniform_number, player_position, player_headshot ";
                commandText += "FROM player_info WHERE player_position = @position";
                command.CommandText = commandText;
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
            }
            
            var playerListJson = JsonSerializer.Serialize(playerList);
            return playerListJson;
        }
        catch (NotSupportedException notSupportedException)
        {
            Console.WriteLine(notSupportedException.Message);
            return notSupportedException.Message;
        }
        catch (NpgsqlException e)
        {
            Console.WriteLine(e);
            return e.Message;
        }
    }
}
