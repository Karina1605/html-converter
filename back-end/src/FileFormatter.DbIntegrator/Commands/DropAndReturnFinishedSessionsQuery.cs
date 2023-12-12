using Dapper;
using FileFormatter.DbIntegrator.Abstractions.Query;
using FileFormatter.DbIntegrator.Options;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace FileFormatter.DbIntegrator.Commands;

public class DropAndReturnFinishedSessionsQuery : IDropAndReturnFinishedSessionsQuery
{
    private readonly string _connectionString;
    private readonly string _tableName;

    public DropAndReturnFinishedSessionsQuery(IOptions<DbConnectionSettings> options)
    {
        _connectionString = options.Value.ConnecitonString;
        _tableName = options.Value.TableName;
    }

    public async Task<IReadOnlyCollection<Guid>> GetAllFinishedByTheMomentSessions(DateTime to, CancellationToken cancellationToken)
    {
        var parameters = new { lastDate = to };
        using var connection = new SqlConnection(_connectionString);
        var query = GetQuery(_tableName);
        var affected = await connection.QueryAsync<Guid>(query, parameters);
        await connection.CloseAsync();
        return affected.ToList();
    }

    private static string GetQuery(string tableName)
    {
        return $@"""
        WITH cte AS(
        SELECT request_id 
        FROM {tableName}
        GROUP BY request_id
        HAVING MAX(last_updated) < @lastDate
        )
        DELETE  FROM {tableName} 
        WHERE request_id IN cte
        RETURNING request_id;
        """;
    }
}
