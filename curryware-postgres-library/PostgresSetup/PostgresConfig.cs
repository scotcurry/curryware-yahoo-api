using Npgsql;

namespace curryware_postgres_library.PostgresSetup;

public abstract class PostgresConfig
{
    public static string GetConnectionString()
    {
        try
        {
            var port = Environment.GetEnvironmentVariable("POSTGRES_SERVER");
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = Environment.GetEnvironmentVariable("POSTGRES_SERVER"),
                Port = Convert.ToInt32(Environment.GetEnvironmentVariable("POSTGRES_PORT")),
                Username = Environment.GetEnvironmentVariable("POSTGRES_USERNAME"),
                Password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD"),
                Database = Environment.GetEnvironmentVariable("POSTGRES_DATABASE")
            };
            return builder.ConnectionString;
        } 
        catch (TypeInitializationException ex)
        {
            Console.WriteLine($"TypeInitialization Error: {ex.Message}");
            throw; // Rethrow to handle higher up, if necessary.
        }
        catch (PostgresConfigException ex)
        {
            Console.WriteLine("Host: " + Environment.GetEnvironmentVariable("POSTGRES_SERVER"));
            Console.WriteLine("Port: " + Environment.GetEnvironmentVariable("POSTGRES_PORT"));
            Console.WriteLine("Username: " + Environment.GetEnvironmentVariable("POSTGRES_USERNAME"));
            Console.WriteLine("Password: " + Environment.GetEnvironmentVariable("POSTGRES_PASSWORD"));
            Console.WriteLine("Database: " + Environment.GetEnvironmentVariable("POSTGRES_DATABASE"));
            
            Console.WriteLine($"Database error: {ex.Message}");
            throw; // Rethrow to handle higher up, if necessary.
        }
    }
}