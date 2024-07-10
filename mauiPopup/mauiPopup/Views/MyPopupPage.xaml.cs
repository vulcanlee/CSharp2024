using CommunityToolkit.Maui.Views;
using mauiPopup.ViewModels;

namespace mauiPopup.Views;

public partial class MyPopupPage : Popup
{
    public MyPopupPage(MyPopupPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        viewModel.ClosePopupHandler = ClosePopup;
    }

    public void ClosePopup()
    {
        MyPopupPageViewModel viewModel = (MyPopupPageViewModel)BindingContext;
        viewModel.ClosePopupHandler = null;
        Close();
    }
}

