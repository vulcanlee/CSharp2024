using CommunityToolkit.Maui.Views;
using mauiPopup.ViewModels;

namespace mauiPopup.Views;

public partial class MyPopupPage : Popup
{
    public MyPopupPage(MyPopupPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

