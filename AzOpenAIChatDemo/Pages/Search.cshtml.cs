using AzOpenAIChatDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;

namespace AzOpenAIChatDemo.Pages
{
    public class SearchModel : PageModel
    {
        private readonly IAzOpenAIService _azOpenAIService;
        private readonly ILogger<SearchModel> _searchLogger;

        public Dictionary<string, string> ConversationHistory { get; set; } = new Dictionary<string, string>();

        public SearchModel(IAzOpenAIService azOpenAIService, ILogger<SearchModel> searchLogger)
        {
            this._azOpenAIService = azOpenAIService;
            this._searchLogger = searchLogger;
        }

        public void OnGet()
        {
        }

        public async void OnPost(string chatInput)
        {
            ConversationHistory.Add("userInput", chatInput);

            var response = _azOpenAIService.GenerateTextAsync(chatInput);
            var content = response.Result.Value.Choices[0].Message.Content;

            StringBuilder sb = new StringBuilder();
            sb.Append("\n\n\n");
            sb.Append(content);
            sb.Append("\n\n\n");

            ConversationHistory.Add("chatResponse", sb.ToString());
        }
    }
}
