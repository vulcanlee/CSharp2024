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
        private readonly ILogger<TogetherMyService> logger;

        public TogetherMyService(ITransientMyService transient,
            IScopedMyService scoped, ISingletonMyService singleton,
            ILogger<TogetherMyService> logger)
        {
            this.transient = transient;
            this.scoped = scoped;
            this.singleton = singleton;
            this.logger = logger;
        }

        public void Show()
        {
            logger.LogInformation($"---------------------------------");
            logger.LogInformation($"TogetherMyService GetHashCode: {GetHashCode()}");
            logger.LogInformation($"---------------------------------");
            logger.LogInformation($"Transient GetHashCode: {transient.GetHashCode()}");
            logger.LogInformation($"Transient Value: {transient.Value}");
            logger.LogInformation($"Transient Create Time: {transient.CreateAt}");
            logger.LogInformation($"---------------------------------");
            logger.LogInformation($"Scoped GetHashCode: {scoped.GetHashCode()}");
            logger.LogInformation($"Scoped Value: {scoped.Value}");
            logger.LogInformation($"Scoped Create Time: {scoped.CreateAt}");
            logger.LogInformation($"---------------------------------");
            logger.LogInformation($"Singleton GetHashCode: {singleton.GetHashCode()}");
            logger.LogInformation($"Singleton Value: {singleton.Value}");
            logger.LogInformation($"Singleton Create Time: {singleton.CreateAt}");
        }
    }
}
