using Dapper;
using FileFormatter.DbIntegrator.Abstractions.Commands;
using FileFormatter.DbIntegrator.Options;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace FileFormatter.DbIntegrator.Commands;

public class AddSessionQuery : IAddSessionQuery
{
    private readonly string _connectionString;
    private readonly string _tableName;

    public AddSessionQuery(IOptions<DbConnectionSettings> options)
    {
        _connectionString = options.Value.ConnecitonString;
        _tableName = options.Value.TableName;
    }

    public async Task AddSessionAsync(Guid requestId, IReadOnlyCollection<string> files, CancellationToken cancellationToken)
    {
        var parameters = files.Select(x => new { request_id = requestId, file_name = x });
        using var connection = new SqlConnection(_connectionString);
        var query = GetQuery(_tableName);
        var affected = await connection.ExecuteAsync(query, parameters);
        if (affected != files.Count)
        {
            throw new Exception("Error during save requestData");
        }
        await connection.CloseAsync();
    }

    private static string GetQuery(string tableName)
    {
        return $@"""
            INSERT INTO {tableName} (requjest_id, file_name, processing_status, generated_file_name, last_updated)
            VALUES(@r_id, @f_name, 0, NULL, now());
        """;
    }
}
