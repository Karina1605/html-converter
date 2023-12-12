namespace FileFormatter.Messaging.Dto;

public record EnqueTaskMessage(
    Guid RequestId,
    string FileName);