using FileFormatter.Messaging.Dto;

namespace FileFormatter.Messaging.Abstractions;

public interface IFileReadyToDownloadProducer
{
    Task OnReadyToDownloadAsync(ProcessingFinalResult linkBuildingResult);
}
