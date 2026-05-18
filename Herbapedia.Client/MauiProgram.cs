using Herbapedia.Client.Servicios;
using Microsoft.Extensions.Logging;

namespace Herbapedia.Client
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

            //Servicios

            builder.Services.AddSingleton<Auth>();
            builder.Services.AddSingleton<APIClient>();
            builder.Services.AddSingleton<AppShell>();
            //Paginas

            builder.Services.AddTransient<LoginPage>();

            return builder.Build();
        }
    }
}
