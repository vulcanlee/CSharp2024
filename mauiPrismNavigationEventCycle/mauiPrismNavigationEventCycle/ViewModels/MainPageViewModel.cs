using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mauiPrismNavigationEventCycle.Services;
using Microsoft.Extensions.Logging;

namespace mauiPrismNavigationEventCycle.ViewModels;

public partial class MainPageViewModel : ObservableObject, INavigatedAware
{
    #region Field Member
    private int _count;
    private readonly INavigationService navigationService;
    private readonly ILogger<LoginPageViewModel> logger;
    private readonly CurrentLogSnapshotService currentLogSnapshotService;
    [ObservableProperty]
    string title = "Main Page";

    [ObservableProperty]
    string text = "Click me";
    #endregion

    #region Property Member
    #endregion

    #region Constructor
    public MainPageViewModel(INavigationService navigationService,
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

    ~MainPageViewModel()
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
    private void Count()
    {
        _count++;
        if (_count == 1)
            Text = "Clicked 1 time";
        else if (_count > 1)
            Text = $"Clicked {_count} times";
    }

    [RelayCommand]
    private async Task GoToSecondPageAsync()
    {
        var naviResult = await navigationService.NavigateAsync("SecondPage");
    }
    [RelayCommand]
    private async Task GoToAllLogsPageAsync()
    {
        var naviResult = await navigationService.NavigateAsync("AllLogsPage");
    }
    [RelayCommand]
    private async Task LogoutAsync()
    {
        var naviResult = await navigationService.NavigateAsync("/LoginPage");
    }
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
