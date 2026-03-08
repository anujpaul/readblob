using System.Text.Json;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Storage.Blobs;
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

}