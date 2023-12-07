using Azure.Storage.Blobs;

namespace AzOpenAIChatDemo.Services
{
    public class CloudFileUploadService : IFileUploadService
    {
        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _configSection;

        public CloudFileUploadService(IConfiguration configuration)
        {
            this._configuration = configuration;
            _configSection = _configuration.GetSection("Storage");
        }

        public List<string> GetUploadedFileNames()
        {
            List<string> lstFileNames = new List<string>();

            string containerName = _configSection.GetValue<string>("Container");
            var blobServiceClient = GetBlobServiceClient();

            BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobs = blobContainerClient.GetBlobs();
            foreach (var blob in blobs)
            {
                lstFileNames.Add(blob.Name);
            }

            return lstFileNames;
        }

        public string UploadFile(IFormFile formFile)
        {
            string containerName = _configSection.GetValue<string>("Container");
            var blobServiceClient = GetBlobServiceClient();

            BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(formFile.FileName);

            using(Stream stream = formFile.OpenReadStream())
            {
                blobClient.Upload(stream, true);
            }

            return formFile.FileName;
        }

        private BlobServiceClient GetBlobServiceClient()
        {
            string accountName = _configSection.GetValue<string>("AccountName");
            string sasToken = _configSection.GetValue<string>("Token");

            string blobUri = $"https://{accountName}.blob.core.windows.net";
            BlobServiceClient blobServiceClient = new BlobServiceClient(new Uri($"{blobUri}{sasToken}"));

            return blobServiceClient;
        }
    }
}
