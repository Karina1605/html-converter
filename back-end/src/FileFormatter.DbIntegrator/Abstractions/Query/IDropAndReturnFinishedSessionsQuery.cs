namespace FileFormatter.DbIntegrator.Abstractions.Query;

public interface IDropAndReturnFinishedSessionsQuery
{
    Task<IReadOnlyCollection<Guid>> GetAllFinishedByTheMomentSessions(
        DateTime to, 
        CancellationToken cancellationToken);
}
