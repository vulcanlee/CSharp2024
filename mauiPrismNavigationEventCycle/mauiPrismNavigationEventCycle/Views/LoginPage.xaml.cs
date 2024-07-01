using mauiPrismNavigationEventCycle.ViewModels;

namespace mauiPrismNavigationEventCycle.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

