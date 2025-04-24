using Blazored.LocalStorage;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using PuffPal.CustomAuth;
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
            // Register service
            builder.Services.AddSingleton<FirebaseService>();

            builder.Services.AddScoped(sp => new FirebaseAuthClient(new FirebaseAuthConfig()
            {
                ApiKey = "",
                AuthDomain = "",
                Providers = [new EmailProvider()]
            }));

            builder.Services.AddBlazoredLocalStorage();

            // Register CustomAuthStateProvider
            builder.Services.AddScoped<CustomAuthStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
            builder.Services.AddScoped(sp => (IAccountManagement)sp.GetRequiredService<AuthenticationStateProvider>());
            builder.Services.AddAuthorizationCore();

            return builder.Build();
        }
    }
}