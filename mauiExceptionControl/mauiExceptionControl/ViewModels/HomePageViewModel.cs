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
    int reloadTimes = 0;
    ReloadPopupPageViewModel reloadPopupPageViewModel;
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
        IPopupService popupService)
    {
        this.navigationService = navigationService;
        this.popupService = popupService;
    }
    #endregion

    #region Method Member
    #region Command Method
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
        if(reloadTimes % 3 != 0)
        {
            for (var i = 0; i < 100; i++)
            {
                Items.Add($"Item {i* reloadTimes}");
            }
            await Task.Delay(100);
        }

        if(items.Count == 0)
        {
            IsEmpty = true;
        }
        Refreshing= false;
        reloadPopupPageViewModel?.ClosePopupHandler();
    }
    #endregion
    #endregion
}
