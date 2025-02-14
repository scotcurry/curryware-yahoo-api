using Npgsql;

namespace curryware_postgres_library.PostgresSetup;

public static class PostgresQueryExecutor
{
    public static async Task<T> ExecuteQueryAsync<T>(Func<NpgsqlConnection, Task<T>> query)
    {
        try
        {
            await using var connection = await PostgresDataSource.DataSource.OpenConnectionAsync();
            return await query(connection);
        }
        catch (TypeInitializationException ex)
        {
            Console.WriteLine($"TypeInitialization Error: {ex.Message}");
            throw; // Rethrow to handle higher up, if necessary.
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database error: {ex.Message}");
            throw; // Rethrow to handle higher up, if necessary.
        }
    }
}