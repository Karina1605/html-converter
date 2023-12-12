using FileFormatter.Common.Enums;

namespace FileFormatter.HubRunner.Contracts.Communication;

public record FileUpdateInfo(
    string FileName, 
    FileProcessingStatus Status,
    string? DownloadLink);
