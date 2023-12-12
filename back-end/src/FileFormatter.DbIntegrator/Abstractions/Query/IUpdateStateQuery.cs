
using FileFormatter.Common.Enums;

namespace FileFormatter.DbIntegrator.Abstractions.Query;

public interface IUpdateStateQuery
{
    Task UpdateFileState(
        Guid requestId, 
        string fileName, 
        FileProcessingStatus fileProcessingStatus,
        string? generatedFileName,
        CancellationToken cancellationToken);
}
