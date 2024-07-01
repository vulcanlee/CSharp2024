using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Prism.Ioc;
using mauiPrismNavigationEventCycle.ViewModels;
using mauiPrismNavigationEventCycle.Views;
using mauiPrismNavigationEventCycle.Services;
using Microsoft.Maui.LifecycleEvents;
using mauiPrismNavigationEventCycle.Extensions;
using System.Diagnostics;

namespace mauiPrismNavigationEventCycle;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UsePrism(prism =>
            {

                prism.RegisterTypes(container =>
                      {
                          container.RegisterForNavigation<MainPage, MainPageViewModel>();
                          container.RegisterForNavigation<LoginPage, LoginPageViewModel>();
                          container.RegisterForNavigation<AllLogsPage, AllLogsPageViewModel>();
                          container.RegisterForNavigation<SecondPage, SecondPageViewModel>();
                          container.RegisterSingleton<CurrentLogSnapshotService>();
                      })
                     .OnInitialized(() =>
                      {
                          // Do some initializations here
                      })
                     .CreateWindow(async navigationService =>
                     {
                         // Navigate to First page of this App
                         var result = await navigationService
                         .NavigateAsync("/LoginPage");
                         if (!result.Success)
                         {
                             var exceptionString = result.Exception.GetFullExceptionMessage();
                             Debug.WriteLine(exceptionString);
                             System.Diagnostics.Debugger.Break();
                         }
                     });
            })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        builder.Logging.AddConsole();

        return builder.Build();
    }
}
