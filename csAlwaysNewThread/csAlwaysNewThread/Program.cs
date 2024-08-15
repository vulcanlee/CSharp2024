namespace csAlwaysNewThread;

internal class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine($"---- 採用非同步的委派方法");
        await Test1();
        Console.WriteLine($"---- 採用同步的委派方法");
        await Test2();
        Console.WriteLine($"---- 採用同步的委派方法與第一層呼叫也是同步、第二層為非同步");
        await Test3();
        Console.WriteLine($"---- 採用同步的委派方法與第一層呼叫也是同步、第二層為同步");
        await Test4();
    }

    static async Task Test1()
    {
        SomeAsyncTask someAsyncTask = new();
        var task = Task.Factory.StartNew(async (x) =>
        {
            await someAsyncTask.Level1Async(1);
        }, CancellationToken.None, TaskCreationOptions.LongRunning);
        Console.WriteLine($"等候 await SomeAsyncTask 非同步工作, " +
            $"TId:{Thread.CurrentThread.ManagedThreadId} " +
            $"(from ThreadPool {Thread.CurrentThread.IsThreadPoolThread})");
        await task.Result;
        Console.WriteLine($"完成 await SomeAsyncTask 非同步工作, " +
            $"TId:{Thread.CurrentThread.ManagedThreadId} " +
            $"(from ThreadPool {Thread.CurrentThread.IsThreadPoolThread})");
    }

    static async Task Test2()
    {
        SomeAsyncTask someAsyncTask = new();
        var task = Task.Factory.StartNew((x) =>
        {
            someAsyncTask.Level1Async(1).Wait();
        }, CancellationToken.None, TaskCreationOptions.LongRunning);
        Console.WriteLine($"等候 await SomeAsyncTask 非同步工作, " +
            $"TId:{Thread.CurrentThread.ManagedThreadId} " +
            $"(from ThreadPool {Thread.CurrentThread.IsThreadPoolThread})");
        await task;
        Console.WriteLine($"完成 await SomeAsyncTask 非同步工作, " +
            $"TId:{Thread.CurrentThread.ManagedThreadId} " +
            $"(from ThreadPool {Thread.CurrentThread.IsThreadPoolThread})");
    }

    static async Task Test3()
    {
        SomeAsyncTask someAsyncTask = new();
        var task = Task.Factory.StartNew((x) =>
        {
            someAsyncTask.Level1(1);
        }, CancellationToken.None, TaskCreationOptions.LongRunning);
        Console.WriteLine($"等候 await SomeAsyncTask 非同步工作, " +
            $"TId:{Thread.CurrentThread.ManagedThreadId} " +
            $"(from ThreadPool {Thread.CurrentThread.IsThreadPoolThread})");
        await task;
        Console.WriteLine($"完成 await SomeAsyncTask 非同步工作, " +
            $"TId:{Thread.CurrentThread.ManagedThreadId} " +
            $"(from ThreadPool {Thread.CurrentThread.IsThreadPoolThread})");
    }

    static async Task Test4()
    {
        SomeAsyncTask someAsyncTask = new();
        var task = Task.Factory.StartNew((x) =>
        {
            someAsyncTask.Level1All(1);
        }, CancellationToken.None, TaskCreationOptions.LongRunning);
        Console.WriteLine($"等候 await SomeAsyncTask 非同步工作, " +
            $"TId:{Thread.CurrentThread.ManagedThreadId} " +
            $"(from ThreadPool {Thread.CurrentThread.IsThreadPoolThread})");
        await task;
        Console.WriteLine($"完成 await SomeAsyncTask 非同步工作, " +
            $"TId:{Thread.CurrentThread.ManagedThreadId} " +
            $"(from ThreadPool {Thread.CurrentThread.IsThreadPoolThread})");
    }
}

public class SomeAsyncTask
{
    public async Task Level1Async(int level)
    {
        Console.WriteLine($"{new string(' ', level * 3)}進入 Level1Async 非同步方法, " +
            $"TId:{Thread.CurrentThread.ManagedThreadId} " +
            $"(from ThreadPool {Thread.CurrentThread.IsThreadPoolThread})");
        await Level2Async(level + 1);
        Console.WriteLine($"{new string(' ', level * 3)}離開 Level1Async 非同步方法, " +
            $"TId:{Thread.CurrentThread.ManagedThreadId} " +
            $"(from ThreadPool {Thread.CurrentThread.IsThreadPoolThread})");
    }

    public async Task Level2Async(int level)
    {
        Console.WriteLine($"{new string(' ', level * 3)}進入 Level2Async 非同步方法, " +
            $"TId:{Thread.CurrentThread.ManagedThreadId} " +
            $"(from ThreadPool {Thread.CurrentThread.IsThreadPoolThread})");
        await Task.Delay(3000);
        Console.WriteLine($"{new string(' ', level * 3)}離開 Level2Async 非同步方法, " +
            $"TId:{Thread.CurrentThread.ManagedThreadId} " +
            $"(from ThreadPool {Thread.CurrentThread.IsThreadPoolThread})");
    }

    public void Level1(int level)
    {
        Console.WriteLine($"{new string(' ', level * 3)}進入 Level1 同步方法, " +
            $"TId:{Thread.CurrentThread.ManagedThreadId} " +
            $"(from ThreadPool {Thread.CurrentThread.IsThreadPoolThread})");
        Level2Async(level + 1).Wait();
        Console.WriteLine($"{new string(' ', level * 3)}離開 Level1 同步方法, " +
            $"TId:{Thread.CurrentThread.ManagedThreadId} " +
            $"(from ThreadPool {Thread.CurrentThread.IsThreadPoolThread})");
    }

    public void Level2(int level)
    {
        Console.WriteLine($"{new string(' ', level * 3)}進入 Level2 同步方法, " +
            $"TId:{Thread.CurrentThread.ManagedThreadId} " +
            $"(from ThreadPool {Thread.CurrentThread.IsThreadPoolThread})");
        Task.Delay(3000).Wait();
        Console.WriteLine($"{new string(' ', level * 3)}離開 Level2 同步方法, " +
            $"TId:{Thread.CurrentThread.ManagedThreadId} " +
            $"(from ThreadPool {Thread.CurrentThread.IsThreadPoolThread})");
    }

    public void Level1All(int level)
    {
        Console.WriteLine($"{new string(' ', level * 3)}進入 Level1All 同步方法, " +
            $"TId:{Thread.CurrentThread.ManagedThreadId} " +
            $"(from ThreadPool {Thread.CurrentThread.IsThreadPoolThread})");
        Level2(level + 1);
        Console.WriteLine($"{new string(' ', level * 3)}離開 Level1All 同步方法, " +
            $"TId:{Thread.CurrentThread.ManagedThreadId} " +
            $"(from ThreadPool {Thread.CurrentThread.IsThreadPoolThread})");
    }

}
