namespace FileFormatter.HubRunner.Abstractions;

public interface ICache
{
    void AddConnection(Guid requestId, string connectionId);

    string GetConnection(Guid requestId);
}
