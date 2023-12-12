namespace FileFormatter.DbIntegrator.Options;

public class DbConnectionSettings
{
    public const string SectionName = nameof(DbConnectionSettings);

    public string ConnecitonString { get; set; }

    public string TableName { get; set; }
}
