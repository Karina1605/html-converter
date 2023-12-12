using FileFormatter.Messaging.Dto;

namespace FileFormatter.Messaging.Abstractions;

public interface IProcessingResultProducer
{
    Task OnFileRocessingFinished(TaskProcessingResult result);
}
