using System.Diagnostics;

namespace csLimitThread;

internal class Program
{
    static SemaphoreSlim semaphore = new SemaphoreSlim(100);
    static async Task Main(string[] args)
    {
        ThreadPool.SetMinThreads(105, 105);
        var allItmes = Enumerable.Range(0, 1000);
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        foreach (var item in allItmes)
        {
            await semaphore.WaitAsync();
            Console.Write("*");
            ThreadPool.QueueUserWorkItem(async (state) =>
            {
                try
                {
                    await Task.Delay(3000);
                }
                finally
                {
                    semaphore.Release();
                    Console.Write("-");
                }
            });
        }
        stopwatch.Stop();
        Console.WriteLine();
        Console.WriteLine($"Time elapsed: {stopwatch.Elapsed}");
    }
}
