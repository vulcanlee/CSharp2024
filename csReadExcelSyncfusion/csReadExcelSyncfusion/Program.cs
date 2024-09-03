using SyncExcel.Services;

namespace csReadExcelSyncfusion
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            ExcleService excleService = new();
            excleService.ReadExcel();
        }
    }
}
