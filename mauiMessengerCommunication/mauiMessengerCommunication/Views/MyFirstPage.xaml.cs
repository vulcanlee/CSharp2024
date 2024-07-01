using mauiMessengerCommunication.ViewModels;

namespace mauiMessengerCommunication.Views;

public partial class MyFirstPage : ContentPage
{
    public MyFirstPage(MyFirstPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

