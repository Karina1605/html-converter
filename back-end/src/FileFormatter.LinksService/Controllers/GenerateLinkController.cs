using FileFormatter.LinksService.Commands;
using FileFormatter.LinksService.Constracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FileFormatter.LinkService.Controllers;

[ApiController]
[Route("generator")]
public class GenerateLinkController
{
    private readonly IMediator _mediator;

    public GenerateLinkController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("generate-link")]
    public async Task<string> GenerateUrl([FromQuery]GenerateLinkRequest request)
    {
        return await _mediator.Send(new GenerateLinkCommand(request.BatchId, request.FileName));
    }
}
