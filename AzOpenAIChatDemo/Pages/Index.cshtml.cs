using AzOpenAIChatDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AzOpenAIChatDemo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IFileUploadService _fileUploadService;
        private readonly IAzCognitiveSearchService _azCognitiveSearchService;

        [TempData]
        public string StatusMessage { get; set; }

        public List<string> UploadedFileNames { get; set; } = new List<string>();

        public IndexModel(ILogger<IndexModel> logger, IFileUploadService fileUploadService, IAzCognitiveSearchService azCognitiveSearchService)
        {
            _logger = logger;
            this._fileUploadService = fileUploadService;
            this._azCognitiveSearchService = azCognitiveSearchService;
        }

        public void OnGet()
        {
            UploadedFileNames = _fileUploadService.GetUploadedFileNames();
        }

        public IActionResult OnPost(IFormFile file)
        {
            try
            {
                if (file == null)
                {
                    StatusMessage = "Select a file to upload.";
                    return Page();
                }

                string uploadedFileName = _fileUploadService.UploadFile(file);
                if (!string.IsNullOrEmpty(uploadedFileName))
                {
                    bool isIndexerSuccess = _azCognitiveSearchService.RunAndCheckIndexer();
                    StatusMessage = isIndexerSuccess ? "FileUploaded and Indexer success." : "An error occurred while indexing document.";
                }
            } 
            catch (Exception ex)
            {
                StatusMessage = "An error occurred while uploading document.";
                _logger.LogError(ex, message: ex.Message);
            }

            return RedirectToPage();
        }
    }
}