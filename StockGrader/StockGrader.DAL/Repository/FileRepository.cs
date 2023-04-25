namespace StockGrader.DAL.Repository
{
    public class FileRepository : IFileRepository
    {
        private const string CommonUserAgent =
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36";
        private const string UserAgentHeader = "User-Agent";
        
        public async Task Fetch(Uri address, string location)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation(UserAgentHeader, CommonUserAgent);
            using var stream = await client.GetStreamAsync(address);

            using StreamReader reader = new(stream);
            var content = reader.ReadToEnd();
            await File.WriteAllTextAsync(location, content);
        }
    }
}
