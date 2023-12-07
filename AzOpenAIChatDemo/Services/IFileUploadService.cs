namespace AzOpenAIChatDemo.Services
{
    public interface IFileUploadService
    {
        string UploadFile(IFormFile formFile);

        List<string> GetUploadedFileNames();
    }
}
