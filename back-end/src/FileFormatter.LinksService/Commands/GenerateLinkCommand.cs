using MediatR;

namespace FileFormatter.LinksService.Commands;

public record GenerateLinkCommand(
    Guid BucketId,
    string FileName) : IRequest<string>;