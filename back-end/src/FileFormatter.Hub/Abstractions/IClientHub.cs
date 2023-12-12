using FileFormatter.HubRunner.Contracts.Communication;
using ConnectionInfo = FileFormatter.HubRunner.Contracts.Communication.ConnectionInfo;

namespace FileFormatter.HubRunner.Abstractions;

public interface IClientHub
{
    Task NotifyUpdate(FileUpdateInfo info, Guid connectionId);

    Task EstablishConnection(ConnectionInfo connectionInfo);
}
