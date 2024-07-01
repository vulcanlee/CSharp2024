using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using mauiMessengerCommunication.Events;
using System.Diagnostics;

namespace mauiMessengerCommunication.ViewModels;

public partial class MainPageViewModel : ObservableObject, INavigatedAware
{
    #region Field Member
    private int _count;
    private readonly INavigationService navigationService;
    CheckSubscripingRequest checkSubscripingRequest = new();
    #endregion

    #region Property Member
    [ObservableProperty]
    string title = "Main Page";

    [ObservableProperty]
    string text = "Click me";

    [ObservableProperty]
    int currentHashCode;

    #endregion

    #region Constructor
    public MainPageViewModel(INavigationService navigationService)
    {
        this.navigationService = navigationService;
        CurrentHashCode = this.GetHashCode();
    }
    #endregion

    #region Method Member
    #region Command Method
    [RelayCommand]
    void CheckSubscriptionTimes()
    {
        WeakReferenceMessenger.Default.Send<CheckSubscripingRequest>(checkSubscripingRequest);
    }
    [RelayCommand]
    void DoNothing()
    {
        WeakReferenceMessenger.Default.Send<DoNothingRequest>();
    }
    [RelayCommand]
    void Logout()
    {
        navigationService.NavigateAsync("/MyFirstPage");
    }
    [RelayCommand]
    void AskUnsubscription()
    {
        WeakReferenceMessenger.Default.Send<AskUnsubscripingRequest>(new AskUnsubscripingRequest()
        {
             HashCode = this.GetHashCode()
        });
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
        WeakReferenceMessenger.Default
            .Register<DoNothingRequest>(this, (sender, message) =>
            {
                Debug.WriteLine($"--------- DoNothingRequest {this.GetHashCode()}");
            });

        WeakReferenceMessenger.Default
            .Register<CheckSubscripingRequest>(this, (sender, MessageHandler) =>
        {
            var hashCodeSender = sender.GetHashCode();
            var hashCode = this.GetHashCode();
            WeakReferenceMessenger.Default.Send(new CheckSubscripingResponse { HashCode = hashCode });
        });

        WeakReferenceMessenger.Default
            .Register<CheckSubscripingResponse>(this, (sender, message) =>
            {
                Debug.WriteLine($"--------- 收到 {this.GetHashCode()} 物件正在訂閱事件中");
            });

        WeakReferenceMessenger.Default.Register<AskUnsubscripingRequest>(this, (sender, message) =>
        {
            var hashCodeSender = sender.GetHashCode();
            var hashCode = this.GetHashCode();
            if (hashCode != message.HashCode)
                WeakReferenceMessenger.Default.Unregister<DoNothingRequest>(this);
        });
    }
    #endregion

    #region Other Method
    #endregion
    #endregion
}
