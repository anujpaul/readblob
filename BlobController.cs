using System.ComponentModel;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api")]
public class BlobController
{

    [HttpGet("test")]
    public string Test()
    {
        return "I am working";
    }

    [HttpPost("readblob")]
    public async Task<string> ReadBlob([FromBody] BlobRequest req)
    {

        var serviceClient = new BlobServiceClient(
            new Uri($"https://{req.StorageAccount}.blob.core.windows.net"),
            new DefaultAzureCredential()
        );

        var containerClient = serviceClient.GetBlobContainerClient(req.ContainerName);

        var blobClient = containerClient.GetBlobClient(req.BlobName);

        var blob = await blobClient.OpenReadAsync();

        var stream = new StreamReader(blob);

        var content = await stream.ReadToEndAsync();

        return content;
    }

    [HttpPost("getContainerProperties")]
    public async Task<BlobContainerProperties> getContainerProperties([FromBody] BlobRequest req)
    {
        IDictionary<string, string> metadata = new Dictionary<string, string>();

        var serviceClient = new BlobServiceClient(
            new Uri($"https://{req.StorageAccount}.blob.core.windows.net"),
            new DefaultAzureCredential()
        );

        var containerClient = serviceClient.GetBlobContainerClient(req.ContainerName);

        var properties = await containerClient.GetPropertiesAsync();

        return properties;
    }

    [HttpPost("setcontainermetadata")]
    public async Task<BlobContainerProperties> setContainerMetadata([FromBody] BlobRequest req)
    {
        IDictionary<string, string> metadata = new Dictionary<string, string>();

        metadata.Add("Belongs_to", "Anuj");

        var blobService = new BlobServiceClient(new Uri($"https://{req.StorageAccount}.blob.core.windows.net"),
                            new DefaultAzureCredential());

        var container = blobService.GetBlobContainerClient(req.ContainerName);

        await container.SetMetadataAsync(metadata);
        var properties = await container.GetPropertiesAsync();
        return properties;
    }

}