using mauiExceptionControl.ViewModels;

namespace mauiExceptionControl.Views;

public partial class SplashPage : ContentPage
{
    public SplashPage(SplashPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

