using System.Diagnostics;

namespace csLimitThread;

internal class Program
{
    static SemaphoreSlim semaphore = new SemaphoreSlim(100, 100);
    static async Task Main(string[] args)
    {
        List<string> usedThreadNumber = new List<string>();
        CancellationTokenSource cts = new CancellationTokenSource();
        new Thread(() =>
        {
            while (cts.IsCancellationRequested == false)
            {
                usedThreadNumber.Insert(0, $"{DateTime.Now} " +
                    $"{Process.GetCurrentProcess().Threads.Count} / {ThreadPool.ThreadCount}");
                usedThreadNumber = usedThreadNumber.Take(1000).ToList();
                Thread.Sleep(100);
            }

        }).Start();
        ThreadPool.SetMinThreads(100, 100);
        var allItmes = Enumerable.Range(0, 1000);
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        foreach (var item in allItmes)
        {
            await semaphore.WaitAsync();
            Console.Write("*");
            ThreadPool.QueueUserWorkItem((state) =>
            {
                try
                {
                    Console.Write("+");
                    Thread.Sleep(3000);
                }
                finally
                {
                    semaphore.Release();
                    Console.Write("-");
                }
            });
        }
        stopwatch.Stop();
        cts.Cancel();
        Console.WriteLine();
        Console.WriteLine($"Time elapsed: {stopwatch.Elapsed}");
        foreach (var item in usedThreadNumber)
        {
            Console.WriteLine(item);
        }
    }
}
