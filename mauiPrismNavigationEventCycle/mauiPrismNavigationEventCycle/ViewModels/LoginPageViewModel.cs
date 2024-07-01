using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mauiPrismNavigationEventCycle.Services;
using Microsoft.Extensions.Logging;

namespace mauiPrismNavigationEventCycle.ViewModels;

public partial class LoginPageViewModel : ObservableObject, INavigatedAware
{
    #region Field Member
    #endregion

    #region Property Member
    private readonly INavigationService navigationService;
    private readonly ILogger<LoginPageViewModel> logger;
    private readonly CurrentLogSnapshotService currentLogSnapshotService;
    #endregion

    #region Constructor
    public LoginPageViewModel(INavigationService navigationService,
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

    ~LoginPageViewModel()
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
    [RelayCommand]
    private async Task LoginAsync()
    {
        var naviResult = await navigationService.NavigateAsync("/NavigationPage/MainPage");
    }
    #endregion

    #region Navigation Event
    public void OnNavigatedFrom(INavigationParameters parameters)
    {
        var message = $"觸發 {this.GetType().Name} > {nameof(OnNavigatedFrom)} " +
            $"(Action:{parameters.GetNavigationMode().ToString()})";
        currentLogSnapshotService.AddLog($"{message}", message=>
        {
            logger.LogInformation(message);
        });
    }

    public void OnNavigatedTo(INavigationParameters parameters)
    {
        var message = $"觸發 {this.GetType().Name} > {nameof(OnNavigatedTo)} " +
            $"(Action:{parameters.GetNavigationMode().ToString()})";
        currentLogSnapshotService.AddLog($"{message}", message =>
        {
            logger.LogInformation(message);
        });
    }
    #endregion

    #region Other Method
    #endregion
    #endregion
}
