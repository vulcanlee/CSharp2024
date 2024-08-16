namespace csSemaphoreSlimExcessiveRelease;

internal class Program
{
    static void Main(string[] args)
    {
        SemaphoreSlim semaphoreSlim1 = new SemaphoreSlim(3, 6);

        List<Task> allTasks = new();

        Execute10Tasks(semaphoreSlim1, allTasks);

        semaphoreSlim1.Release();
        semaphoreSlim1.Release();
        semaphoreSlim1.Release();
        Execute10Tasks(semaphoreSlim1, allTasks);
    }

    private static void Execute10Tasks(SemaphoreSlim semaphoreSlim1, List<Task> allTasks)
    {
        allTasks.Clear();
        for (int i = 0; i < 10; i++)
        {
            int index = i;
            Console.WriteLine($"Index{index} " +
                $"semaphore 剩下 {semaphoreSlim1.CurrentCount}");

            semaphoreSlim1.Wait();
            var task = Task.Run(() =>
            {
                Console.WriteLine($"{DateTime.Now.TimeOfDay} 工作 {index} " +
                    $"獲得 semaphore({semaphoreSlim1.CurrentCount})");
                Thread.Sleep(3000);
                semaphoreSlim1.Release();
                Console.WriteLine($"{DateTime.Now.TimeOfDay} 工作 {index} " +
                    $"釋放 semaphore({semaphoreSlim1.CurrentCount})");
            });
            allTasks.Add(task);
        }

        Task.WaitAll(allTasks.ToArray());
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
    }
}
