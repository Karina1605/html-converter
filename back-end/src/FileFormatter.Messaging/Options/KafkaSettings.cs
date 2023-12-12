namespace FileFormatter.Messaging.Options;

public class KafkaSettings
{
    public string Broker { get; set; } =string.Empty;

    public string NewRequestTopic { get; set; } = string.Empty;

    public string FileProcessedTopic { get; set; } = string.Empty;

    public string GeneratedLinksTopic { get; set; } = string.Empty;

    public string GroupId { get; set; } = string.Empty;
}
