using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace mauiExceptionToclipboard.ViewModels;

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
    async Task CleanClipboard()
    {
        await Clipboard.Default.SetTextAsync(null);
    }
    [RelayCommand]
    void ThrowUnhandleException()
    {
        throw new Exception("喔喔，這裡發生例外異常");
    }

    [RelayCommand]
    void ThrowUnhandleAggregateException()
    {
        var exceptions = new List<Exception>();

        exceptions.Add(new ArgumentException("Argument Exception Message"));
        exceptions.Add(new NullReferenceException("Null Reference Exception Message"));

        throw new AggregateException("Aggregate Exception Message", exceptions);
    }

    [RelayCommand]
    void ThrowUnhandleInnerException()
    {
        try
        {
            throw new Exception("喔喔，這裡發生例外異常");
        }

        catch (ArgumentException e)
        {
            //make sure this path does not exist
            if (File.Exists("file://Bigsky//log.txt%22)%20==%20false") == false)
            {
                throw new FileNotFoundException("File Not found when trying to write argument exception to the file", e);
            }
        }
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
