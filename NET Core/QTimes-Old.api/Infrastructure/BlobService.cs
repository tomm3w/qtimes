using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace QTimes.api.Infrastructure
{
    public class BlobService : IBlobService
    {

        private readonly BlobContainerClient _client;
        public BlobService(IConfiguration configuration)
        {
            string connectionString = configuration["BlobConnectionString"];
            BlobServiceClient blobServiceClient = new BlobServiceClient(configuration["BlobConnectionString"]);
            _client = blobServiceClient.GetBlobContainerClient(configuration["ContainerName"]);
        }
        public string GetFileUrl(string fileName)
        {
            return _client.GetBlobClient(fileName).Uri.ToString();
        }


        public async Task<string> UploadFile(Stream stream, string fileName)
        {
            var blobClient = _client.GetBlobClient(fileName);
            await blobClient.UploadAsync(stream, true);
            return blobClient.Uri.ToString();
        }
    }

}
