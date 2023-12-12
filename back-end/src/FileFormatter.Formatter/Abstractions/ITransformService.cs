namespace FileFormatter.Formatter.Abstractions;

public interface ITransformService
{
    Task<Stream> ConvertToPdf(string url);
}
