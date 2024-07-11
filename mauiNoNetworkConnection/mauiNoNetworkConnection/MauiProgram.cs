using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Prism.Ioc;
using mauiNoNetworkConnection.ViewModels;
using mauiNoNetworkConnection.Views;

namespace mauiNoNetworkConnection;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        AppDomain.CurrentDomain.UnhandledException += (s, e) =>
        {
            var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("UnhandledException");
            logger.LogError(e.ExceptionObject as Exception, "Unhandled Exception");
            Clipboard.Default.SetTextAsync(e.ExceptionObject.ToString()).Wait();
        };

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UsePrism(prism =>
            {

                prism.RegisterTypes(container =>
                      {
                          container.RegisterForNavigation<MainPage, MainPageViewModel>();
                      })
                     .OnInitialized(() =>
                      {
                          // Do some initializations here
                      })
                     .CreateWindow(async navigationService =>
                     {
                         // Navigate to First page of this App
                         var result = await navigationService
                         .NavigateAsync("NavigationPage/MainPage");
                         if (!result.Success)
                         {
                             System.Diagnostics.Debugger.Break();
                         }
                     });
            })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Logging.AddConsole();

        return builder.Build();
    }
}
