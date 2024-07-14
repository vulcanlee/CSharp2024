using mauiExceptionControl.ViewModels;

namespace mauiExceptionControl.Views;

public partial class HomePage : ContentPage
{
    public HomePage(HomePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

