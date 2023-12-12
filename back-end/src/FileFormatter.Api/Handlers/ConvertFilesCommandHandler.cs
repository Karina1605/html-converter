using FileFormatter.Api.Commands;
using FileFormatter.DbIntegrator.Abstractions.Commands;
using FileFormatter.Messaging.Abstractions;
using FileFormatter.StoreIntegrator.Abstractions;
using MediatR;

namespace FileFormatter.Api.Handlers;

public class ConvertFilesCommandHandler : IRequestHandler<ConvertFilesCommand, Guid>
{
    private readonly IStoreService _storeService;
    private readonly IAddSessionQuery _addSessionQuery;
    private readonly INewRequestProducer _newRequestProducer;

    public ConvertFilesCommandHandler(
        IStoreService storeService,
        IAddSessionQuery addSessionQuery,
        INewRequestProducer newRequestProducer)
    {
        _storeService = storeService;
        _addSessionQuery = addSessionQuery;
        _newRequestProducer = newRequestProducer;
    }

    public async Task<Guid> Handle(ConvertFilesCommand request, CancellationToken cancellationToken)
    {
        var guid = Guid.NewGuid();

        await _storeService.AddBucket(
            guid, 
            request.FilesToConvert.ToDictionary(
                x => x.FileName, 
                x => x.OpenReadStream()));
        
        await _addSessionQuery.AddSessionAsync(
            guid, 
            request.FilesToConvert.Select(x => x.Name).ToList(), 
            cancellationToken);
        
        await _newRequestProducer.EnqueNewFilesBatch(
            guid, 
            request.FilesToConvert.Select(x => x.Name).ToList(), 
            cancellationToken);

        return guid;
    }
}
