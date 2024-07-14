using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace mauiExceptionControl.ViewModels;

public partial class SplashPageViewModel : ObservableObject, INavigatedAware
{
    #region Field Member
    private readonly INavigationService navigationService;

    string endPoint = "https://wwwww.google.com";
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
    async Task RetryFailUrl()
    {
        IsShowProcess = true;
        IsShowError = false;
        ExceptionMessage = "";
        endPoint = "https://wwwww.google.com";
        await Initialization();
    }
    [RelayCommand]
    async Task Retry()
    {
        IsShowProcess = true;
        IsShowError = false;
        ExceptionMessage = "";
        endPoint = "https://www.google.com";
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
        int totalProcessItems = 100;
        for (var i = 1; i <= totalProcessItems; i++)
        {
            CurrentProcessWidth = (1.0 * i / totalProcessItems) * ProcessWidth;
            ProcessText = $"{100.0 * i / totalProcessItems:0.#}%";
            await Task.Delay(100);

            if (i == 65)
            {
                try
                {
                    var foo = await new HttpClient().GetStringAsync(endPoint);
                }
                catch (Exception ex)
                {
                    await Clipboard.Default.SetTextAsync(ex.ToString());
                    ExceptionMessage = $"啟動發生問題 : {ex.Message}";
                    IsShowProcess = false;
                    IsShowError = true;

                    #region Toast
                    string text = "Toast : 請將剪貼簿內容，Email 給系統管理者 / " + $"({DateTime.Now})";
                    ToastDuration duration = ToastDuration.Short;
                    double fontSize = 14;

                    var toast = Toast.Make(text, duration, fontSize);

                    await toast.Show();
                    #endregion
                    break;
                }
            }
        }

        if (IsShowError == false)
        {
            await navigationService.NavigateAsync("/NavigationPage/HomePage");
        }
    }
    #endregion
    #endregion
}
