using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

namespace mauiPopup.ViewModels;

public partial class MainPageViewModel : ObservableObject, INavigatedAware
{
    #region Field Member
    private int _count;
    private readonly INavigationService navigationService;
    private readonly IPopupService popupService;
    MyPopupPageViewModel myPopupViewModel;
    #endregion

    #region Property Member
    [ObservableProperty]
    string title = "Main Page";

    [ObservableProperty]
    string text = "Click me";
    #endregion

    #region Constructor
    public MainPageViewModel(INavigationService navigationService,
        IPopupService popupService)
    {
        this.navigationService = navigationService;
        this.popupService = popupService;
    }
    #endregion

    #region Method Member
    #region Command Method
    [RelayCommand]
    private void ShowPopup()
    {
        popupService.ShowPopupAsync<MyPopupPageViewModel>();
    }
    [RelayCommand]
    private async void Count()
    {
        _count++;
        if (_count == 1)
            Text = "Clicked 1 time";
        else if (_count > 1)
            Text = $"Clicked {_count} times";
        popupService.ShowPopup<MyPopupPageViewModel>(onPresenting: viewmodel =>
        {
            myPopupViewModel = viewmodel;
            double getWidth = DeviceDisplay.Current.MainDisplayInfo.Width /
            DeviceDisplay.Current.MainDisplayInfo.Density;
            double getHeight = DeviceDisplay.Current.MainDisplayInfo.Height /
            DeviceDisplay.Current.MainDisplayInfo.Density;
            viewmodel.SetSize(getWidth, getHeight);
        });

        for (int i = 1; i <= 10; i++)
        {
            await Task.Delay(1000);
            myPopupViewModel.Message = i.ToString();
        }

        myPopupViewModel?.ClosePopupHandler?.Invoke();
    }
    #endregion

    #region Navigation Event
    public void OnNavigatedFrom(INavigationParameters parameters)
    {
        Debug.WriteLine("------------------------ OnNavigatedFrom");
    }

    public void OnNavigatedTo(INavigationParameters parameters)
    {
        Debug.WriteLine("------------------------ OnNavigatedTo");
    }
    #endregion

    #region Other Method
    #endregion
    #endregion
}
