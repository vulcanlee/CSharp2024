using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace mauiExceptionControl.ViewModels;

public partial class SplashPageViewModel : ObservableObject, INavigatedAware
{
    #region Field Member
    private readonly INavigationService navigationService;

    int failRetryTimes = 0;
    int maxFailRetryTimes = 3;
    #endregion

    #region Property Member
    [ObservableProperty]
    double processWidth = 300;
    [ObservableProperty]
    double currentProcessWidth = 0;
    [ObservableProperty]
    string processText = "";
    [ObservableProperty]
    bool isShowProcess = true;
    [ObservableProperty]
    bool isShowError = false;
    [ObservableProperty]
    string exceptionMessage = "";

    #endregion

    #region Constructor
    public SplashPageViewModel(INavigationService navigationService)
    {
        this.navigationService = navigationService;
    }
    #endregion

    #region Method Member
    #region Command Method
    [RelayCommand]
    async Task Retry()
    {
        IsShowProcess = true;
        IsShowError = false;
        ExceptionMessage = "";
        await Initialization();
    }
    #endregion

    #region Navigation Event
    public void OnNavigatedFrom(INavigationParameters parameters)
    {
    }

    public async void OnNavigatedTo(INavigationParameters parameters)
    {
        await Initialization();
        
    }
    async Task Initialization()
    {
        failRetryTimes++;
        int totalProcessItems = 100;
        for (var i = 1; i <= totalProcessItems; i++)
        {
            CurrentProcessWidth = (1.0 * i / totalProcessItems) * ProcessWidth;
            ProcessText = $"{100.0 * i / totalProcessItems:0.#}%";
            await Task.Delay(100);

            if (failRetryTimes< maxFailRetryTimes)
            {
                if (i == 65)
                {
                    ExceptionMessage = "An error occurred while initializing the application 初始化應用程式時發生錯誤";
                    IsShowProcess = false;
                    IsShowError = true;
                    break;
                }
            }
        }

        if(IsShowError == false)
        {
            await navigationService.NavigateAsync("/NavigationPage/HomePage");
        }
    }
    #endregion
    #endregion
}
