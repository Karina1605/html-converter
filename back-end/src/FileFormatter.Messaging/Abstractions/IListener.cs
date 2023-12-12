namespace FileFormatter.Messaging.Abstractions;

public interface IListener
{
    Task Run(CancellationToken cancellationToken);
}
