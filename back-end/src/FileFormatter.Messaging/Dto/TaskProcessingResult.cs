using FileFormatter.Common.Enums;

namespace FileFormatter.Messaging.Dto;

public record TaskProcessingResult(
    Guid RequestId,
    FileProcessingStatus Result,
    string OriginalFileName,
    string? NewFileName);