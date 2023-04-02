namespace StockGrader.Infrastructure.Repository
{
    public class FileRepository : IFileRepository
    {
        public async Task Fetch(Uri address, string location)
        {
            var client = new HttpClient();
            var stream = await client.GetStreamAsync(address);

            using StreamReader reader = new StreamReader(stream);
            var content = reader.ReadToEnd();
            File.WriteAllText(location, content);
        }
    }
}
