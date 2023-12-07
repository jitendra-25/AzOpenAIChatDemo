using Azure;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;

namespace AzOpenAIChatDemo.Services
{
    public class AzCognitiveSearchService : IAzCognitiveSearchService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AzCognitiveSearchService> _logger;

        public AzCognitiveSearchService(IConfiguration configuration, ILogger<AzCognitiveSearchService> logger)
        {
            this._configuration = configuration;
            this._logger = logger;
        }

        public bool RunAndCheckIndexer()
        {
            try
            {
                string indexerName = _configuration["SearchIndexerName"];

                SearchIndexerClient indexerClient = new SearchIndexerClient(
                                    new Uri(_configuration["SearchServiceEndpoint"]),
                                    new AzureKeyCredential(_configuration["SearchApiKey"]));

                Response indexerResponse = indexerClient.RunIndexer(indexerName);
                if (indexerResponse != null)
                {
                    if (indexerResponse.Status == 202)
                    {
                        Thread.Sleep(5000);

                        if (CheckIndexerStatus(indexerClient, indexerName))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: ex.InnerException?.Message);
                throw;
            }
        }

        private bool CheckIndexerStatus(SearchIndexerClient searchIndexerClient, string indexerName)
        {
            bool isIndexerSuccess = false;
            SearchIndexerStatus execInfo = searchIndexerClient.GetIndexerStatus(indexerName);
            IndexerExecutionResult lastResult = execInfo.LastResult;

            if (lastResult.ErrorMessage != null)
            {
                _logger.LogError($"Indexer failed. Error = {lastResult.ErrorMessage}");
            }
            else
            {
                _logger.LogInformation("Indexer Success");
                isIndexerSuccess = true;
            }
            return isIndexerSuccess;
        }
    }
}
