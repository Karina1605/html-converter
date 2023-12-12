using FileFormatter.Api.Commands;
using FileFormatter.Api.Requests.Post;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FileFormatter.Api.Controllers;

[ApiController]
public class ConvertFileController
{
    private readonly IMediator _mediator;

    public ConvertFileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("convert-files")]
    public async Task<ConvertFilesResponse> AcceptFilesForConvertation(IReadOnlyCollection<IFormFile> files)
    {
        var generatedSessionId = await _mediator.Send(new ConvertFilesCommand(files));
        return new ConvertFilesResponse(generatedSessionId);
    }
}
