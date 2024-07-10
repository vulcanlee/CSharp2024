namespace mauiStatusBar.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        //CommunityToolkit.Maui.Core.Platform.StatusBar.SetColor(Color.FromArgb("#1190ca")); 
        //CommunityToolkit.Maui.Core.Platform.StatusBar.SetStyle(CommunityToolkit.Maui.Core.StatusBarStyle.LightContent);
        base.OnNavigatedTo(args);
    }
}

