﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using mauiPopup.Events;

namespace mauiPopup.ViewModels;

public partial class MyPopupPageViewModel : ObservableObject, INavigatedAware
{
    #region Field Member
    private readonly INavigationService navigationService;
    #endregion

    #region Property Member
    public Action ClosePopupHandler { get; set; }
    [ObservableProperty]
    string message = "0";
    [ObservableProperty]
    double dialogWidth = 300.0;
    [ObservableProperty]
    double dialogHeight = 200.0;
    #endregion

    #region Constructor
    public MyPopupPageViewModel(INavigationService navigationService)
    {
        this.navigationService = navigationService;
        DialogWidth = DeviceDisplay.Current.MainDisplayInfo.Width /
     DeviceDisplay.Current.MainDisplayInfo.Density;
        DialogHeight = DeviceDisplay.Current.MainDisplayInfo.Height /
        DeviceDisplay.Current.MainDisplayInfo.Density;
    }
    #endregion

    #region Method Member
    #region Command Method
    [RelayCommand]
    void Empty()
    {
    }
    [RelayCommand]
    void Close()
    {
        WeakReferenceMessenger.Default.Send<PopupEvent>(new PopupEvent()
        {
            Now = DateTime.Now
        });
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
