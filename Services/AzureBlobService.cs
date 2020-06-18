using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace GitHubTelemetry.Services
{
    /// <summary>
    /// Uploading reports to blob storage
    /// </summary>
    public class AzureBlobService
    {
        public const string ENV_STORAGE_CONNECTION = "STORAGE_CONNECTION";

        private IConfiguration configuration;

        public AzureBlobService(IConfiguration injectedConfiguration)
        {
            configuration = injectedConfiguration;
        }

        /// <summary>
        /// Takes generated file and uploads it to specified blob container
        /// </summary>
        /// <param name="filePath"></param>
        public void UploadBlob(string filePath)
        {
            // Set an environment variable with your storage connection string as the value
            string connectionString = Environment.GetEnvironmentVariable(ENV_STORAGE_CONNECTION);

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(configuration["azure_blob_container"]);
            BlobClient blobClient = blobContainerClient.GetBlobClient(filePath);

            Console.WriteLine($"Uploading to Blob storage: {blobClient.Uri}");

            using (FileStream uploadFileStream = File.OpenRead(filePath))
                blobClient.Upload(uploadFileStream);

            Console.WriteLine($"Uploaded to Blob storage!");
        }
    }
}
