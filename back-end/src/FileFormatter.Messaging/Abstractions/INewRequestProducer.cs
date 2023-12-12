namespace FileFormatter.Messaging.Abstractions;

public interface INewRequestProducer
{
    Task EnqueNewFilesBatch(Guid requestId, IReadOnlyCollection<string> filesNames, CancellationToken cancellationToken);
    
}
