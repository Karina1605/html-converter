using Dapper;
using FileFormatter.Common.Enums;
using FileFormatter.DbIntegrator.Abstractions.Query;
using FileFormatter.DbIntegrator.Options;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace FileFormatter.DbIntegrator.Commands;

public class UpdateStateQuery : IUpdateStateQuery
{
    private readonly string _connectionString;
    private readonly string _tableName;

    public UpdateStateQuery(IOptions<DbConnectionSettings> options)
    {
        _connectionString = options.Value.ConnecitonString;
        _tableName = options.Value.TableName;
    }

    public async Task UpdateFileState(
        Guid requestId, 
        string fileName, 
        FileProcessingStatus fileProcessingStatus, 
        string? generatedFileName, 
        CancellationToken cancellationToken)
    {
        var parameters = new {
            processingStatus = (int)fileProcessingStatus, 
            generatedFile = generatedFileName,
            requestId = requestId,
            fileName = fileName,
        };
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await connection.ExecuteScalarAsync(GetQuery(_tableName), parameters);
        await connection.CloseAsync();
    }

    private static string GetQuery(string tableName)
    {
        return $@"""
        UPDATE {tableName}
        SET 
        processing_status = @processingStatus,
        generated_file_name = @generatedFile,
        last_updated = now()
        WHERE requjest_id = @requestId AND file_name =@fileName;
        """;
    }
}
