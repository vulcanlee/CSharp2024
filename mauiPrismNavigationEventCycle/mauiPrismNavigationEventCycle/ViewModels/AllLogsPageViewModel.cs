using Android.Util;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mauiPrismNavigationEventCycle.Models;
using mauiPrismNavigationEventCycle.Services;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace mauiPrismNavigationEventCycle.ViewModels;

public partial class AllLogsPageViewModel : ObservableObject, INavigatedAware
{
    #region Field Member
    #endregion

    #region Property Member
    private readonly INavigationService navigationService;
    private readonly ILogger<LoginPageViewModel> logger;
    private readonly CurrentLogSnapshotService currentLogSnapshotService;

    [ObservableProperty]
    ObservableCollection<MessageItem> messageItems = new ObservableCollection<MessageItem>();
    #endregion

    #region Constructor
    public AllLogsPageViewModel(INavigationService navigationService,
        ILogger<LoginPageViewModel> logger,
        CurrentLogSnapshotService currentLogSnapshotService)
    {
        this.navigationService = navigationService;
        this.logger = logger;
        this.currentLogSnapshotService = currentLogSnapshotService;

        var message = $"物件建構中 {this.GetType().Name}";
        currentLogSnapshotService.AddLog($"{message}", message =>
        {
            logger.LogInformation(message);
        });
    }

    ~AllLogsPageViewModel()
    {
        var message = $"物件 【解構】 中 {this.GetType().Name}";
        currentLogSnapshotService.AddLog($"{message}", message =>
        {
            logger.LogInformation(message);
        });
    }
    #endregion

    #region Method Member
    #region Command Method
    #endregion

    #region Navigation Event
    public void OnNavigatedFrom(INavigationParameters parameters)
    {
        var message = $"觸發 {this.GetType().Name} > {nameof(OnNavigatedFrom)} " +
            $"(Action:{parameters.GetNavigationMode().ToString()})";
        currentLogSnapshotService.AddLog($"{message}", message =>
        {
            logger.LogInformation(message);
        });
    }

    public void OnNavigatedTo(INavigationParameters parameters)
    {
        var message = $"觸發 {this.GetType().Name} > {nameof(OnNavigatedFrom)} " +
            $"(Action:{parameters.GetNavigationMode().ToString()})";
        currentLogSnapshotService.AddLog($"{message}", message =>
        {
            logger.LogInformation(message);
        });

        message = $"開始建立 Log 集合物件";
        currentLogSnapshotService.AddLog($"{message}", message =>
        {
            logger.LogInformation(message);
        });

        List<MessageItem> items = new List<MessageItem>();
        foreach (var item in currentLogSnapshotService.CurrentLogs)
        {
            var log = new MessageItem()
            {
                Message = item
            };
            //items.Add(log);
            MessageItems.Add(log);
        }
        //MessageItems = new ObservableCollection<MessageItem>(items);
        message = $"完成建立 Log 集合物件";
        currentLogSnapshotService.AddLog($"{message}", message =>
        {
            logger.LogInformation(message);
        });
        var foo = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}{Environment.NewLine}{message}";
        MessageItems.Insert(0, new MessageItem()
        {
            Message = foo
        });
    }
    #endregion

    #region Other Method
    #endregion
    #endregion
}
