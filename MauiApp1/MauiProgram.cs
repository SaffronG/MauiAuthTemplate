using MauiApp1.Services;
using Microsoft.Extensions.Logging;

namespace MauiApp1
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<ViewModels.MainPageViewModel>();

            builder.Services.AddSingleton<IAuthClient, AuthClient>();
            builder.Services.AddSingleton<IShellClient, ShellClient>();
            builder.Services.AddSingleton<Duende.IdentityModel.OidcClient.Browser.IBrowser, MauiAuthenticatorBrowser>();

            return builder.Build();
        }
    }
}
