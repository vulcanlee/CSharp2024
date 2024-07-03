using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading;
using CommunityToolkit.Maui.Alerts;

namespace mauiAlertSnackbarToast.ViewModels;

public partial class MainPageViewModel : ObservableObject, INavigatedAware
{
    #region Field Member
    private int _count;
    private readonly INavigationService navigationService;

    #endregion

    #region Property Member
    [ObservableProperty]
    string title = "Main Page";

    [ObservableProperty]
    string text = "Click me";
    #endregion

    #region Constructor
    public MainPageViewModel(INavigationService navigationService)
    {
        this.navigationService = navigationService;
    }
    #endregion

    #region Method Member
    #region Command Method
    [RelayCommand]
    async Task ShowSnakebar()
    {
        var snackbarOptions = new SnackbarOptions
        {
            BackgroundColor = Colors.Red,
            TextColor = Colors.Green,
            ActionButtonTextColor = Colors.Yellow,
            CornerRadius = new CornerRadius(10),
            Font = Microsoft.Maui.Font.SystemFontOfSize(14),
            ActionButtonFont = Microsoft.Maui.Font.SystemFontOfSize(14),
            CharacterSpacing = 0.5
        };

        string text = "This is a Snackbar" + $"({DateTime.Now})";
        string actionButtonText = "Click Here to Dismiss";
        TimeSpan duration = TimeSpan.FromSeconds(3);

        var snackbar = Snackbar.Make(text, null, actionButtonText, duration, snackbarOptions);

        await snackbar.Show();
    }

    [RelayCommand]
    async Task ShowToast()
    {
        string text = "This is a Toast "+$"({DateTime.Now})";
        ToastDuration duration = ToastDuration.Short;
        double fontSize = 14;

        var toast = Toast.Make(text, duration, fontSize);

        await toast.Show();
    }
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
    #endregion

    #region Other Method
    #endregion
    #endregion
}
