using mauiPrismNavigationEventCycle.ViewModels;

namespace mauiPrismNavigationEventCycle.Views;

public partial class AllLogsPage : ContentPage
{
    public AllLogsPage(AllLogsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

