namespace FileFormatter.LinksService.Constracts;

public record GenerateLinkRequest(Guid BatchId, string FileName);