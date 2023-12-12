namespace FileFormatter.Formatter.Abstractions;

public interface ISingleFileProcessor
{
    Task ProcessNextAsync(Guid batchId, string fileName);
}
