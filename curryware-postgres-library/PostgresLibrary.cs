using System.Text.Json;
using curryware_postgres_library.Models.PlayerModels;
using Npgsql;

namespace curryware_postgres_library;

public class PostgresLibrary
{
    public static async Task<string> GetPlayerIdsByPosition(string position = "QB")
    {
        var postgresUserName = Environment.GetEnvironmentVariable("POSTGRES_USERNAME");
        var postgresPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
        var playerList = new List<PlayerModel>();
        try
        {
            var connectionStringBuilder = new NpgsqlConnectionStringBuilder();
            connectionStringBuilder.Host = "ubuntu-postgres.curryware.org";
            connectionStringBuilder.Port = 5432;
            connectionStringBuilder.Username = postgresUserName;
            connectionStringBuilder.Password = postgresPassword;
            connectionStringBuilder.Database = "currywarefantasy";
            var connectionString = connectionStringBuilder.ConnectionString;
            
            await using var dataSource = NpgsqlDataSource.Create(connectionString); 
            await using (var conn = await dataSource.OpenConnectionAsync())
            {
                var command = conn.CreateCommand();
                var commandText = "SELECT player_id, player_season_key, player_name, player_url, ";
                commandText += "player_team, player_bye_week, player_uniform_number, player_position, player_headshot ";
                commandText += "FROM player_info";
                command.CommandText = commandText;
                
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var byeWeek = await reader.IsDBNullAsync(reader.GetInt32(reader.GetOrdinal("player_bye_week")));
                    var playerId = reader.GetInt32(reader.GetOrdinal("player_id"));
                    var playerSeasonKey = await reader.IsDBNullAsync(reader.GetOrdinal("player_season_key")) ? 
                        reader.GetString(reader.GetOrdinal("player_season_key")): null;
                    var playerName = await reader.IsDBNullAsync(reader.GetOrdinal("player_name")) ?
                        reader.GetString(reader.GetOrdinal("player_name")): null;
                    var playerUrl = await reader.IsDBNullAsync(reader.GetOrdinal("player_url"))
                        ? reader.GetString(reader.GetOrdinal("player_url")) : null;
                    var playerTeam = await reader.IsDBNullAsync(reader.GetOrdinal("player_team")) ?
                        reader.GetString(reader.GetOrdinal("player_team")) : null;
                    var playerByeWeek = await reader.IsDBNullAsync(reader.GetOrdinal("player_bye_week")) ?
                        reader.GetInt32(reader.GetOrdinal("player_bye_week")) : (int?)null;
                    var playerUniformNumber = await reader.IsDBNullAsync(reader.GetOrdinal("player_uniform_number")) ?
                        reader.GetInt32(reader.GetOrdinal("player_uniform_number")) : (int?)null;
                    var playerPosition = await reader.IsDBNullAsync(reader.GetOrdinal("player_position")) ?
                        reader.GetString(reader.GetOrdinal("player_position")) : null;
                    var playerHeadshot = await reader.IsDBNullAsync(reader.GetOrdinal("player_headshot")) ?
                        reader.GetString(reader.GetOrdinal("player_headshot")) : null;

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
