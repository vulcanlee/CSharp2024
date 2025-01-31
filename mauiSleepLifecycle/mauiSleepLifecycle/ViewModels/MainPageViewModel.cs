using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace mauiSleepLifecycle.ViewModels;

public partial class MainPageViewModel : ObservableObject, INavigatedAware, IApplicationLifecycleAware
{
    #region Field Member
    private int _count;
    private readonly INavigationService navigationService;
    private readonly ILogger<MainPageViewModel> logger;
    CancellationTokenSource cts;
    #endregion

    #region Property Member
    [ObservableProperty]
    string title = "Main Page";

    [ObservableProperty]
    string text = "Click me";

    [ObservableProperty]
    ObservableCollection<string> items = new ObservableCollection<string>();

    List<string> executionLogs = new List<string>();
    #endregion

    #region Constructor
    public MainPageViewModel(INavigationService navigationService,
        ILogger<MainPageViewModel> logger)
    {
        this.navigationService = navigationService;
        this.logger = logger;
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
    #endregion

    #region Navigation Event
    public void OnNavigatedFrom(INavigationParameters parameters)
    {
    }

    public void OnNavigatedTo(INavigationParameters parameters)
    {
    }

    public void OnResume()
    {
        cts?.Cancel();
        executionLogs.Insert(0, $"Resume : {DateTime.Now.ToString()}");
        logger.LogInformation($"Resume : {DateTime.Now.ToString()}");
        Items.Clear();
        foreach (var log in executionLogs)
        {
            Items.Add(log);
        }
    }

    public async void OnSleep()
    {
        executionLogs.Clear();
        cts = new CancellationTokenSource();
        try
        {
            while (!cts.IsCancellationRequested)
            {
                executionLogs.Insert(0, $"Sleep : {DateTime.Now.ToString()}");
                logger.LogInformation($"Sleep : {DateTime.Now.ToString()}");
                await Task.Delay(1000, cts.Token);
            }
        }
        catch (Exception)
        {
        }
    }
    #endregion

    #region Other Method
    #endregion
    #endregion
}
