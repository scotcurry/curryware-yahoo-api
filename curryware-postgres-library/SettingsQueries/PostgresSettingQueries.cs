using System.Text.Json;
using curryware_postgres_library.Models.SettingsModels;
using curryware_postgres_library.PostgresSetup;
using Npgsql;

namespace curryware_postgres_library.SettingsQueries;

public abstract class PostgresSettingQueries
{
    public static async Task<string> GetSystemInformation()
    {
         return await PostgresQueryExecutor.ExecuteQueryAsync(async connection =>
        {
            var playerList = new List<SystemInformationModel>();
            const string selectString = """
                                        
                                                        SELECT system_id, system_value_name, system_value
                                                        FROM system_information
                                        """;

            await using var command = new NpgsqlCommand(selectString, connection);

            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var systemId = reader.GetInt32(reader.GetOrdinal("system_id"));
                var systemValueName = reader.GetString(reader.GetOrdinal("system_value_name"));
                var systemValue = reader.GetString(reader.GetOrdinal("system_value"));

                var playerModel = new SystemInformationModel
                {
                    SystemId = systemId,
                    SettingName = systemValueName,
                    SettingValue = systemValue
                };

                playerList.Add(playerModel);
            }

            return JsonSerializer.Serialize(playerList);
        });
    }
}