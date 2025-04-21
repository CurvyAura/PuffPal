using Microsoft.Extensions.Logging;
using PuffPal.Services;

namespace PuffPal
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
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif
            // Register services
            builder.Services.AddSingleton<UserService>();
            builder.Services.AddSingleton<ProgressService>();
            builder.Services.AddSingleton<AchievementService>();
            builder.Services.AddSingleton<DailyLogService>();
            builder.Services.AddSingleton<QuoteService>(); 

            return builder.Build();
        }
    }
}