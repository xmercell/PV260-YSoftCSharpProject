namespace StockGrader.Infrastructure.Repository
{
    public interface IFileRepository
    {
        /// <summary>
        /// Download the web resource and save it into the file system folder.
        /// </summary>
        /// <param name="address">Web resource</param>
        /// <param name="location">File system folder location</param>
        Task Fetch(Uri address, string location);
    }
}
