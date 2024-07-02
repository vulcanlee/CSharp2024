using mauiSoftKeyboardVisibility.ViewModels;

namespace mauiSoftKeyboardVisibility.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        this.Focused += (s, e) =>
        {
            this.HideSoftInputOnTapped = true;
        };
    }

    protected override void OnAppearing()
    {
        MainPageViewModel MainPageViewModel = this.BindingContext as MainPageViewModel;
        MainPageViewModel.HiddenSoftKeyboardHandle = HiddenSoftKeyboard;
        base.OnAppearing();
    }

    public void HiddenSoftKeyboard()
    {
        this.HideSoftInputOnTapped = true;
    }
}

