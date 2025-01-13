namespace csDIRegistrationDifferent
{
    public class MyService: ITransientMyService, ISingletonMyService, IScopedMyService
    {
        public int Value { get; set; }
        public DateTime CreateAt { get; set; }
        public MyService()
        {
            Value = new Random().Next(1000, 9999);
            CreateAt = DateTime.Now;
        }
    }


    public interface ISingletonMyService
    {
        public int Value { get; set; }
        public DateTime CreateAt { get; set; }
    }
    public interface IScopedMyService : ISingletonMyService
    {
    }

    public interface ITransientMyService : ISingletonMyService
    {
    }

    public class TogetherMyService
    {
        private readonly ITransientMyService transient;
        private readonly IScopedMyService scoped;
        private readonly ISingletonMyService singleton;
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<TogetherMyService> logger;

        public TogetherMyService(ITransientMyService transient,
            IScopedMyService scoped, ISingletonMyService singleton,
            IServiceProvider serviceProvider,
            ILogger<TogetherMyService> logger)
        {
            this.transient = transient;
            this.scoped = scoped;
            this.singleton = singleton;
            this.serviceProvider = serviceProvider;
            this.logger = logger;
        }

        public void Show()
        {
            var transientBy服務定位器 = serviceProvider.GetService<ITransientMyService>();
            var scopedBy服務定位器 = serviceProvider.GetService<IScopedMyService>();
            var singletonBy服務定位器 = serviceProvider.GetService<ISingletonMyService>();

            Console.WriteLine($"---  [From TogetherMyService ] ------------------------------");
            Console.WriteLine($"TogetherMyService GetHashCode: {GetHashCode()}");
            Console.WriteLine($"-------------------------------------------------------------");
            Console.WriteLine($"Transient GetHashCode: {transient.GetHashCode()}");
            Console.WriteLine($"Transient Value: {transient.Value}");
            Console.WriteLine($"Transient服務定位器 Value: {transientBy服務定位器!.Value}");
            Console.WriteLine($"Transient Create Time: {transient.CreateAt}");
            Console.WriteLine($"---------------------------------");
            Console.WriteLine($"Scoped GetHashCode: {scoped.GetHashCode()}");
            Console.WriteLine($"Scoped Value: {scoped.Value}");
            Console.WriteLine($"Scoped服務定位器 Value: {scopedBy服務定位器!.Value}");
            Console.WriteLine($"Scoped Create Time: {scoped.CreateAt}");
            Console.WriteLine($"---------------------------------");
            Console.WriteLine($"Singleton GetHashCode: {singleton.GetHashCode()}");
            Console.WriteLine($"Singleton Value: {singleton.Value}");
            Console.WriteLine($"Singleto服務定位器n Value: {singletonBy服務定位器!.Value}");
            Console.WriteLine($"Singleton Create Time: {singleton.CreateAt}");
            Console.WriteLine($"");
            //Console.WriteLine($"---  [From Service Locator of TogetherMyService ] ------------------------------");
            //Console.WriteLine($"*********************************");
            //Console.WriteLine($"Transient GetHashCode: {transient.GetHashCode()}");
            //Console.WriteLine($"Transient Value: {transient.Value}");
            //Console.WriteLine($"Transient Create Time: {transient.CreateAt}");
            //Console.WriteLine($"---------------------------------");
            //Console.WriteLine($"Scoped GetHashCode: {scoped.GetHashCode()}");
            //Console.WriteLine($"Scoped Value: {scoped.Value}");
            //Console.WriteLine($"Scoped Create Time: {scoped.CreateAt}");
            //Console.WriteLine($"---------------------------------");
            //Console.WriteLine($"Singleton GetHashCode: {singleton.GetHashCode()}");
            //Console.WriteLine($"Singleton Value: {singleton.Value}");
            //Console.WriteLine($"*********************************");
            Console.WriteLine($"");
            Console.WriteLine($"");
            Console.WriteLine($"");
        }
    }
}
