using Npgsql;

namespace curryware_postgres_library.PostgresSetup;

public static class PostgresDataSource
{
    static PostgresDataSource()
    {
        DataSource = NpgsqlDataSource.Create(PostgresConfig.GetConnectionString());
    }

    public static NpgsqlDataSource DataSource { get; }
}