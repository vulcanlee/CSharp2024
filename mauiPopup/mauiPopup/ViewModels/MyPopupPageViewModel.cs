using CommunityToolkit.Mvvm.ComponentModel;
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
    double width = 300;
    [ObservableProperty]
    double height = 300;
    [ObservableProperty]
    string message = "0";
    #endregion

    #region Constructor
    public MyPopupPageViewModel(INavigationService navigationService)
    {
        this.navigationService = navigationService;
    }
    #endregion

    #region Method Member
    #region Command Method
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
    public void SetSize(double width, double height)
    {
        Width = width;
        Height = height;
    }
    #endregion
    #endregion
}
