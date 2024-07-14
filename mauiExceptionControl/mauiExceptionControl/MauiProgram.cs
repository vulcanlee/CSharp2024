using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Prism.Ioc;
using mauiExceptionControl.ViewModels;
using mauiExceptionControl.Views;

namespace mauiExceptionControl;

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
                          container.RegisterForNavigation<SplashPage, SplashPageViewModel>();
                          container.RegisterForNavigation<HomePage, HomePageViewModel>();
                      })
                     .OnInitialized(() =>
                      {
                          // Do some initializations here
                      })
                     .CreateWindow(async navigationService =>
                     {
                         // Navigate to First page of this App
                         var result = await navigationService
                         .NavigateAsync("SplashPage");
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

        builder.Services.AddTransientPopup<ReloadPopupPage, ReloadPopupPageViewModel>();

        builder.Logging.AddConsole();

        return builder.Build();
    }
}
