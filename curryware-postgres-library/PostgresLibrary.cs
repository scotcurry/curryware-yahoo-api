using Npgsql;

namespace curryware_postgres_library;

public class PostgresLibrary
{
    public static async Task<string> GetPlayerIdsByPosition(string position = "QB")
    {
        try
        {
            var connectionStringBuilder = new NpgsqlConnectionStringBuilder();
            connectionStringBuilder.Host = "ubuntu-postgres.curryware.org";
            connectionStringBuilder.Port = 5432;
            connectionStringBuilder.Username = "scot";
            connectionStringBuilder.Password = "****";
            connectionStringBuilder.Database = "currywarefantasy";
            string connectionString = connectionStringBuilder.ConnectionString;
            
            await using var dataSource = NpgsqlDataSource.Create(connectionString);
            using (var conn = dataSource.OpenConnection())
            {
                var command = conn.CreateCommand();
                command.CommandText = "SELECT player_id FROM player_info";
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var playerId = reader.GetInt32(0);
                    Console.WriteLine(playerId);
                }
            }
        }
        catch (NotSupportedException notSupportedException)
        {
            Console.WriteLine(notSupportedException.Message);
        }
        catch (NpgsqlException e)
        {
            Console.WriteLine(e);
        }
        return "debug";
    }
}
