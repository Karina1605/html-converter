using MediatR;

namespace FileFormatter.Api.Commands;

public record ConvertFilesCommand(
    IReadOnlyCollection<IFormFile> FilesToConvert) : IRequest<Guid>;