using Azure;
using Azure.AI.OpenAI;

namespace AzOpenAIChatDemo.Services
{
    public interface IAzOpenAIService
    {
        Task<Response<ChatCompletions>> GenerateTextAsync(string chatInput);
    }
}
