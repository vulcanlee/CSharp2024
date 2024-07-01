using Project9.ViewModels;

namespace Project9.Views;

public partial class MainPage : ContentPage
{
    bool firstTime = true;
    public MainPage()
    {
        InitializeComponent();

        this.HideSoftInputOnTapped = true;
    }

    protected override void OnAppearing()
    {
        MainPageViewModel viewModel = this.BindingContext as MainPageViewModel;
        viewModel.TurnOffSoftKeyboard = TurnOffKeyboard;
        base.OnAppearing();
    }

    async void TurnOffKeyboard()
    {
        this.HideSoftInputOnTapped=true;
        //await Task.Delay(300);
        //this.HideSoftInputOnTapped = true;
        //if (firstTime)
        //{
        //    await Task.Delay(300);
        //    this.HideSoftInputOnTapped = false;
        //    await Task.Delay(300);
        //    this.HideSoftInputOnTapped = true;

        //    firstTime = false;
        //}
    }
}

