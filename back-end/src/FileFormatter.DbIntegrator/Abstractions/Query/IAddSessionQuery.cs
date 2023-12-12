namespace FileFormatter.DbIntegrator.Abstractions.Commands;

public interface IAddSessionQuery
{
    Task AddSessionAsync(
        Guid requestId, 
        IReadOnlyCollection<string> files, 
        CancellationToken cancellationToken);
}
