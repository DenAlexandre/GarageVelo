using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using GarageVelo.Services;
using GarageVelo.Services.Mock;
using GarageVelo.Services.Api;
using GarageVelo.ViewModels;
using GarageVelo.Views;

namespace GarageVelo;

public static class MauiProgram
{
    // Toggle: set to true to use the real API backend, false for mock services
    private const bool USE_API = false;
    private const string API_BASE_URL = "https://localhost:5001";

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMauiMaps()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Session service is always the same (SecureStorage-based)
        builder.Services.AddSingleton<ISessionService, MockSessionService>();

        if (USE_API)
        {
            // Authenticated HttpClient with JWT header
            builder.Services.AddTransient<AuthenticatedHttpClientHandler>();
            builder.Services.AddHttpClient("GarageVeloApi", client =>
            {
                client.BaseAddress = new Uri(API_BASE_URL);
            }).AddHttpMessageHandler<AuthenticatedHttpClientHandler>();

            // API service implementations
            builder.Services.AddSingleton<IAuthService>(sp =>
            {
                var factory = sp.GetRequiredService<IHttpClientFactory>();
                var session = sp.GetRequiredService<ISessionService>();
                return new ApiAuthService(factory.CreateClient("GarageVeloApi"), session);
            });
            builder.Services.AddSingleton<IGarageService>(sp =>
            {
                var factory = sp.GetRequiredService<IHttpClientFactory>();
                return new ApiGarageService(factory.CreateClient("GarageVeloApi"));
            });
            builder.Services.AddSingleton<ISubscriptionService>(sp =>
            {
                var factory = sp.GetRequiredService<IHttpClientFactory>();
                return new ApiSubscriptionService(factory.CreateClient("GarageVeloApi"));
            });
            builder.Services.AddSingleton<IPaymentService>(sp =>
            {
                var factory = sp.GetRequiredService<IHttpClientFactory>();
                return new ApiPaymentService(factory.CreateClient("GarageVeloApi"));
            });
        }
        else
        {
            // Mock implementations (in-memory, no backend required)
            builder.Services.AddSingleton<IAuthService, MockAuthService>();
            builder.Services.AddSingleton<IGarageService, MockGarageService>();
            builder.Services.AddSingleton<ISubscriptionService, MockSubscriptionService>();
            builder.Services.AddSingleton<IPaymentService, MockPaymentService>();
        }

        // NFC placeholder (phase 2)
        builder.Services.AddSingleton<INfcService, MockNfcService>();

        // ViewModels
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<RegisterViewModel>();
        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<ScanViewModel>();
        builder.Services.AddTransient<GarageDetailViewModel>();
        builder.Services.AddTransient<SubscriptionViewModel>();
        builder.Services.AddTransient<ProfileViewModel>();

        // Pages
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<ScanPage>();
        builder.Services.AddTransient<GarageDetailPage>();
        builder.Services.AddTransient<SubscriptionPage>();
        builder.Services.AddTransient<ProfilePage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
