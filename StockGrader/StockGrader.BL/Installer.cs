using Microsoft.Extensions.DependencyInjection;
using StockGrader.BL.Writer;

namespace StockGrader.BL
{
    public static class Installer
    {
        public static void InstallBl(this IServiceCollection collection) 
        {
            collection.AddTransient<IDiffProvider, DiffProvider>();
            collection.AddTransient<IWriter, ConsoleWriter>();
        }
    }
}
