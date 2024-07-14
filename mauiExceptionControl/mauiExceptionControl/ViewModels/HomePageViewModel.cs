using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace mauiExceptionControl.ViewModels;

public partial class HomePageViewModel : ObservableObject, INavigatedAware
{
    #region Field Member
    private readonly INavigationService navigationService;
    private readonly IPopupService popupService;
    private readonly IPageDialogService pageDialogService;
    int reloadTimes = 0;
    ReloadPopupPageViewModel reloadPopupPageViewModel;
    string endPoint = "https://wwwww.google.com";

    #endregion

    #region Property Member
    [ObservableProperty]
    ObservableCollection<string> items = new ObservableCollection<string>();
    [ObservableProperty]
    bool isEmpty = true;
    [ObservableProperty]
    bool refreshing = false;
    #endregion

    #region Constructor
    public HomePageViewModel(INavigationService navigationService,
        IPopupService popupService,
        IPageDialogService pageDialogService)
    {
        this.navigationService = navigationService;
        this.popupService = popupService;
        this.pageDialogService = pageDialogService;
    }
    #endregion

    #region Method Member
    #region Command Method
    [RelayCommand]
    void SetIncorrectUrl()
    {
        endPoint = "https://wwwww.google.com";
    }
    [RelayCommand]
    void SetCorrectUrl()
    {
        endPoint = "https://www.google.com";
    }
    [RelayCommand]
    async Task Reload()
    {
        await ReloadData();
    }
    #endregion

    #region Navigation Event
    public void OnNavigatedFrom(INavigationParameters parameters)
    {
    }

    public async void OnNavigatedTo(INavigationParameters parameters)
    {
        await ReloadData();
    }
    #endregion

    #region Other Method
    async Task ReloadData()
    {
        IsEmpty = false;
        popupService.ShowPopup<ReloadPopupPageViewModel>(onPresenting: (viewModel) =>
        {
            reloadPopupPageViewModel = viewModel;
        });
        await Task.Delay(100);
        reloadTimes++;
        Items.Clear();

        try
        {
            var foo = await new HttpClient().GetStringAsync(endPoint);
        }
        catch (Exception ex)
        {
            await Clipboard.Default.SetTextAsync(ex.ToString());
            reloadPopupPageViewModel?.ClosePopupHandler();
            await pageDialogService.DisplayAlertAsync("錯誤", $"發生錯誤 : {ex.Message}", "確定");
            IsEmpty = true;
            Refreshing = false;

            #region Snackbar
            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Colors.LightGray,
                TextColor = Colors.Red,
                ActionButtonTextColor = Colors.Yellow,
                CornerRadius = new CornerRadius(10),
                Font = Microsoft.Maui.Font.SystemFontOfSize(14),
                ActionButtonFont = Microsoft.Maui.Font.SystemFontOfSize(14),
                CharacterSpacing = 0.5
            };

            string text = "Snackbar : 請將剪貼簿內容，Email 給系統管理者 / " + $"({DateTime.Now})";
            string actionButtonText = "點選這裡，取消通知";
            TimeSpan duration = TimeSpan.FromSeconds(3);

            var snackbar = Snackbar.Make(text, null, actionButtonText, duration, snackbarOptions);

            await snackbar.Show();
            #endregion
            return;
        }

        if (reloadTimes % 3 != 0)
        {
            for (var i = 0; i < 100; i++)
            {
                Items.Add($"Item {i * reloadTimes}");
            }
            await Task.Delay(100);
        }

        if (items.Count == 0)
        {
            IsEmpty = true;
        }
        Refreshing = false;
        reloadPopupPageViewModel?.ClosePopupHandler();
    }
    #endregion
    #endregion
}
