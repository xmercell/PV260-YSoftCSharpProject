using StockGrader.Evaluator;
using StockGrader.DAL.Repository;
using StockGrader.Writer;
using System.Reflection;


// TODO: configure from out (appsettings or something like that - not hardcoded)
var filePath = Path.Combine(Assembly.GetExecutingAssembly().Location, "..\\..\\..\\..\\..\\StockGrader.Test\\TestFiles\\ARK_ORIGINAL.csv");
var url = new Uri("https://ark-funds.com/wp-content/uploads/funds-etf-csv/ARK_INNOVATION_ETF_ARKK_HOLDINGS.csv");

// TODO: use DI
var fileRepository = new FileRepository();
IStockRepository stockRepository = new StockRepository(fileRepository, url, filePath);

var oldReport = stockRepository.GetLast();
await stockRepository.FetchNew();
var newReport = stockRepository.GetLast();

var diffProvider = new DiffProvider();
diffProvider.CalculateDiff(oldReport.Entries, newReport.Entries);

var writer = new ConsoleWriter(diffProvider);

Console.WriteLine(writer.NewText);
Console.WriteLine(writer.IncreasedText);
Console.WriteLine(writer.ReducedText);
Console.WriteLine(writer.UnchangedText);
Console.WriteLine(writer.RemovedText);
