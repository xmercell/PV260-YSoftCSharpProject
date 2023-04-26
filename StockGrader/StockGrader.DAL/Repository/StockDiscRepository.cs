﻿using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Azure.Cosmos;
using StockGrader.DAL.Exception;
using StockGrader.DAL.Model;
using System.Globalization;

namespace StockGrader.DAL.Repository
{
    public class StockDiscRepository : IStockRepository
    {
        private readonly Uri _holdingsSheetUri;
        private readonly string _userAgentHeader;
        private readonly string _commonUserAgent;

        public StockDiscRepository(Uri holdingsSheetUri, string userAgentHeader, string commonUserAgent)
        {
            _holdingsSheetUri = holdingsSheetUri;
            _userAgentHeader = userAgentHeader;
            _commonUserAgent = commonUserAgent;
        }

        public async Task FetchNew()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation(_userAgentHeader, _commonUserAgent);
            try
            {
                using var stream = await client.GetStreamAsync(_holdingsSheetUri);
            
                using StreamReader reader = new(stream);
                var content = reader.ReadToEnd();


                // Create a new item to store in the container
                var jsonObject = new
                {
                    id = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    content
                };
                //connect to db
                var cosmosClient = new CosmosClient("AccountEndpoint=https://matejgros.documents.azure.com:443/;AccountKey=JyWaRohZpb39OzO4z6VbVNrN16EKK1WkN6yCdP5OXjD9h9IWP1iIvHRbcI5UJn8YJsJmapJpd97HACDbtulQyg==");
                var database = cosmosClient.GetDatabase("ysoft");
                var container = database.GetContainer("ysoft_con");
                // Insert the item into the container
                var result = await container.CreateItemAsync(jsonObject);


            }
            catch (HttpRequestException)
            {
                throw new NewStocksNotAvailableException(_holdingsSheetUri);
            }
            catch (IOException ex)
            {
                throw new NewStocksNotAvailableException(ex);
            }
        }

        public StockReport GetLast()
        {
            try
            {
                var cosmosClient = new CosmosClient("AccountEndpoint=https://matejgros.documents.azure.com:443/;AccountKey=JyWaRohZpb39OzO4z6VbVNrN16EKK1WkN6yCdP5OXjD9h9IWP1iIvHRbcI5UJn8YJsJmapJpd97HACDbtulQyg==");

                // Get a reference to the database and container
                var database = cosmosClient.GetDatabase("ysoft");
                var container = database.GetContainer("ysoft_con");

                // Create a query to get the last entry from the container
                var query = new QueryDefinition("SELECT TOP 1 c.content FROM c ORDER BY c.date DESC");
                var iterator = container.GetItemQueryIterator<dynamic>(query);

                // Get the content of the last entry from the iterator
                dynamic result = iterator.ReadNextAsync().Result.FirstOrDefault();
                string content = result?.content ?? string.Empty;

                // If there is no content, throw an exception
                if (string.IsNullOrEmpty(content))
                {
                    throw new LastStockNotFoundException("No content found in the Cosmos DB container.");
                }
                using var reader = new StringReader(content);
                using var csv = new CsvReader(reader, GetConfig());
                csv.Context.RegisterClassMap(new ReportEntryMap());

                var rows = csv.GetRecords<ReportEntry>().ToList();
                return new StockReport { Entries = rows };
            }
            catch (IOException ex)
            {
                throw new LastStockNotFoundException(ex);
            }
        }

        private CsvConfiguration GetConfig()
        {
            return new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null,
                ShouldSkipRecord = (row) =>
                {
                    if (row.Row.HeaderRecord != null)
                    {
                        return !DateTime.TryParseExact(row.Row.GetField("date"), 
                                "MM/dd/yyyy",
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.None,
                                out _);
                    }
                    return false;
                }
            };
        }
    }
}
