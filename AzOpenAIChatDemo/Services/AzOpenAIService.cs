using Azure;
using Azure.AI.OpenAI;

namespace AzOpenAIChatDemo.Services
{
    public class AzOpenAIService : IAzOpenAIService
    {
        private readonly ILogger<AzOpenAIService> _logger;
        private readonly IConfiguration _configuration;
        private readonly OpenAIClient _client;

        private ChatCompletionsOptions _options;

        private readonly string _azOpenAIApiBase = string.Empty;
        private readonly string _azOpenAIKey = string.Empty;
        private readonly string _searchServiceEndpoint = string.Empty;
        private readonly string _searchIndexName = string.Empty;
        private readonly string _searchApiKey = string.Empty;
        private readonly string _azOpenAIDeploymentId = string.Empty;
        
        public AzOpenAIService(ILogger<AzOpenAIService> logger, IConfiguration configuration)
        {
            this._logger = logger;
            this._configuration = configuration;

            _azOpenAIApiBase = _configuration.GetValue<string>("AzOpenAIApiBase");
            _azOpenAIKey = _configuration.GetValue<string>("AzOpenAIKey");
            _azOpenAIDeploymentId = _configuration.GetValue<string>("AzOpenAIDeploymentId");

            _searchServiceEndpoint = _configuration.GetValue<string>("SearchServiceEndpoint");
            _searchIndexName = _configuration.GetValue<string>("SearchIndexName");
            _searchApiKey = _configuration.GetValue<string>("SearchApiKey");

            _client = new OpenAIClient(new Uri(_azOpenAIApiBase), new AzureKeyCredential(_azOpenAIKey));
            CreateChatCompletionOptions();
        }

        public async Task<Response<ChatCompletions>> GenerateTextAsync(string chatInput)
        {
            List<ChatMessage> messages = new List<ChatMessage>()
            {
                new ChatMessage(ChatRole.User, chatInput)
            };

            InitializeMessages(messages);
            var result = await _client.GetChatCompletionsAsync(_azOpenAIDeploymentId, _options);
            return result;
        }

        private void CreateChatCompletionOptions()
        {
            _options = new ChatCompletionsOptions()
            {
                AzureExtensionsOptions = new AzureChatExtensionsOptions()
                {
                    Extensions =
                    {
                        new AzureCognitiveSearchChatExtensionConfiguration()
                        {
                            SearchEndpoint = new Uri(_searchServiceEndpoint),
                            IndexName = _searchIndexName,
                            SearchKey = new AzureKeyCredential(_searchApiKey)
                        }
                    }
                }
            };
        }

        private void InitializeMessages(List<ChatMessage> chatMessages)
        {
            foreach(var chatMessage in chatMessages)
            {
                _options.Messages.Add(chatMessage);
            }
        }
    }
}
