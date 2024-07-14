using CommunityToolkit.Maui.Views;
using mauiExceptionControl.ViewModels;

namespace mauiExceptionControl.Views;

public partial class ReloadPopupPage : Popup
{
    public ReloadPopupPage(ReloadPopupPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        viewModel.ClosePopupHandler = ClosePopup;
    }


    public void ClosePopup()
    {
        ReloadPopupPageViewModel viewModel = (ReloadPopupPageViewModel)BindingContext;
        viewModel.ClosePopupHandler = null;
        Close();
    }
}

