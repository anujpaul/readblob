public class BlobRequest
{
    public string? StorageAccount { get; set; }
    public string? ContainerName { get; set; }
    public string? BlobName { get; set; }
    public string? MetaDataKey { get; set; }
    public string? MetaDataValue { get; set; }
}