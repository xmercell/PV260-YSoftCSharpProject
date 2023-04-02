using StockGrader.Domain.Model;
using StockGrader.Evaluator;
using StockGrader.Writer;
using System.Reflection;

var FilePathOld = Path.Combine(Assembly.GetExecutingAssembly().Location, "..\\..\\..\\..\\..\\StockGrader.Test\\TestFiles\\ARK_ORIGINAL.csv");
var FilePathNew = Path.Combine(Assembly.GetExecutingAssembly().Location, "..\\..\\..\\..\\..\\StockGrader.Test\\TestFiles\\ARK_ORIGINAL.csv");

StockReport stockReportOld = new StockReport(FilePathOld);
StockReport stockReportNew = new StockReport(FilePathNew);

DiffProvider diffProvider = new DiffProvider();
diffProvider.CalculateDiff(stockReportOld.Entries, stockReportNew.Entries);

var writer = new ConsoleWriter(diffProvider);

Console.WriteLine(writer.NewText);
Console.WriteLine(writer.IncreasedText);
Console.WriteLine(writer.ReducedText);
Console.WriteLine(writer.UnchangedText);
Console.WriteLine(writer.RemovedText);
