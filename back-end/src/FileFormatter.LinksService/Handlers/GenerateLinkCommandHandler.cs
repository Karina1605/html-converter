using FileFormatter.LinksService.Abstractions;
using FileFormatter.LinksService.Commands;
using MediatR;

namespace FileFormatter.LinksService.Handlers;

public class GenerateLinkCommandHandler : IRequestHandler<GenerateLinkCommand, string>
{
    private readonly ILinkBuilderService _linkBuilderService;

    public GenerateLinkCommandHandler(ILinkBuilderService linkBuilderService)
    {
        _linkBuilderService = linkBuilderService;
    }

    public async Task<string> Handle(GenerateLinkCommand request, CancellationToken cancellationToken)
    {
        return await _linkBuilderService.GenerateDownloadLink(request.BucketId, request.FileName);
    }
}
