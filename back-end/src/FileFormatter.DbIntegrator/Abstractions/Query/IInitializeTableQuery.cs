namespace FileFormatter.DbIntegrator.Abstractions.Query;

public interface IInitializeTableQuery
{
    Task CreateTableIfNotExistsAsync(CancellationToken cancellationToken);
}
