using FileFormatter.Formatter.Abstractions;
using FileFormatter.LinksService.Constracts;
using FileFormatter.Messaging.Abstractions;
using FileFormatter.StoreIntegrator.Abstractions;
using Flurl;
using Flurl.Http;

namespace FileFormatter.Formatter;

public class SingleFileProcessor : ISingleFileProcessor
{
    private readonly ITransformService _transformService;
    private readonly IStoreService _storeService;
    private readonly IProcessingResultProducer _resultProducer;
    private readonly ILinkBuilderClient _linkBuilderClient;

    public SingleFileProcessor(
        ITransformService transformService,
        IStoreService storeService,
        IProcessingResultProducer resultProducer,
        ILinkBuilderClient linkBuilderClient)
    {
        _transformService = transformService;
        _storeService = storeService;
        _resultProducer = resultProducer;
        _linkBuilderClient = linkBuilderClient;
    }

    public async Task ProcessNextAsync(Guid batchId, string fileName)
    {
        var downloadLink = await _linkBuilderClient.GenerateLinkAsync(new GenerateLinkRequest(batchId, fileName));

        try
        {
            var stream = await _transformService.ConvertToPdf(downloadLink);
            var newFileName = fileName.Replace(".html", ".pdf");
            await _storeService.AddFileToStorage(batchId, newFileName, stream);
            await _resultProducer.OnFileRocessingFinished(
                new Messaging.Dto.TaskProcessingResult(
                    batchId,
                    Common.Enums.FileProcessingStatus.Converted,
                    fileName,
                    newFileName));
        }
        catch (Exception ex)
        {
            new Messaging.Dto.TaskProcessingResult(
                   batchId,
                   Common.Enums.FileProcessingStatus.ProcessingFailed,
                   fileName,
                   null);
        }
    }
}
