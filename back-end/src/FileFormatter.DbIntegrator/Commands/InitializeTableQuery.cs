using Dapper;
using FileFormatter.DbIntegrator.Abstractions.Query;
using FileFormatter.DbIntegrator.Options;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace FileFormatter.DbIntegrator.Commands;

public class InitializeTableQuery : IInitializeTableQuery
{
    private readonly string _tableName;
    private readonly string _connectionString;

    public InitializeTableQuery(IOptions<DbConnectionSettings> options)
    {
        _tableName = options.Value.TableName;
        _connectionString = options.Value.ConnecitonString;
    }

    public async Task CreateTableIfNotExistsAsync(CancellationToken cancellationToken)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await connection.ExecuteScalarAsync(GetQuery(_tableName), cancellationToken);
        await connection.CloseAsync();
    }

    private static string GetQuery(string tableName) {
        return @$"""
        CREATE TABLE IF NOT EXISTS
        {tableName}(
            requjest_id uuid DEFAULT uuid_generate_v4 (),
            file_name VARCHAR NOT NULL,
            processing_status INTEGER NOT NULL,
            generated_file_name VARCHAR NULL,
            last_updated DATE,
            PRIMARY KEY (request_id)
        );""";
    }
    
}
