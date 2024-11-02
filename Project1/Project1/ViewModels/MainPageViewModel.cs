using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Project1.ViewModels;

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
    private void Count()
    {
        // DEBUG & MYCUSTOM 符號同時有被定義，顯示 "Hello, World! >> DEBUG & MYCUSTOM"
#if DEBUG && MYCUSTOM
        Console.WriteLine("Hello, World! >> DEBUG & MYCUSTOM");
#endif
#if DEBUG && !MYCUSTOM
        Console.WriteLine("Hello, World! >> DEBUG & !MYCUSTOM");
#endif
#if DEBUG
        Console.WriteLine("Hello, World! >> DEBUG");
#endif
#if MYCUSTOM
        Console.WriteLine("Hello, World! >> MYCUSTOM");
#endif

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
