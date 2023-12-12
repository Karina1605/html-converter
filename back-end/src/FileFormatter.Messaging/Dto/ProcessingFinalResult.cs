using FileFormatter.Common.Enums;

namespace FileFormatter.Messaging.Dto;

public record ProcessingFinalResult(
    Guid RequestId, 
    string OriginalFileName,
    FileProcessingStatus Status,
    string? DownloadLink);
