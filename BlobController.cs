using System.ComponentModel;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Http.HttpResults;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;

[ApiController]
[Route("api")]
public class BlobController : ControllerBase
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
    public async Task<IActionResult> getContainerProperties([FromBody] BlobRequest req)
    {
        IDictionary<string, string> metadata = new Dictionary<string, string>();

        var serviceClient = new BlobServiceClient(
            new Uri($"https://{req.StorageAccount}.blob.core.windows.net"),
            new DefaultAzureCredential()
        );

        var containerClient = serviceClient.GetBlobContainerClient(req.ContainerName);

        var properties = await containerClient.GetPropertiesAsync();

        return Ok(properties);
    }

    [HttpPost("setcontainermetadata")]
    public async Task<IActionResult> setContainerMetadata([FromBody] BlobRequest req)
    {
        IDictionary<string, string> metadata = new Dictionary<string, string>();

        if (string.IsNullOrEmpty(req.MetaDataKey) || string.IsNullOrEmpty(req.MetaDataValue))
        {
            return BadRequest("Metadata key and value cannot be null or empty");
        }
        metadata.Add(req.MetaDataKey, req.MetaDataValue);

        var blobService = new BlobServiceClient(new Uri($"https://{req.StorageAccount}.blob.core.windows.net"),
                            new DefaultAzureCredential());

        var container = blobService.GetBlobContainerClient(req.ContainerName);

        await container.SetMetadataAsync(metadata);
        var properties = await container.GetPropertiesAsync();
        return Ok(properties);
    }

    [HttpPost("deletecontainermetadata")]
    public async Task<IActionResult> deleteContainerMetada([FromBody] BlobRequest req)
    {
        var container = new BlobServiceClient(
            new Uri($"https://{req.StorageAccount}.blob.core.windows.net"),
            new DefaultAzureCredential()
        ).GetBlobContainerClient(req.ContainerName);

        var properties = await container.GetPropertiesAsync();

        var metadata = properties.Value.Metadata;

        if (metadata.ContainsKey(req.MetaDataKey))
        {
            metadata.Remove(req.MetaDataKey);
            await container.SetMetadataAsync(metadata);
        }

        var updatedProperty = await container.GetPropertiesAsync();


        return Ok(updatedProperty);
    }

}