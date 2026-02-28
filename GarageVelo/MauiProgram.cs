using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using GarageVelo.Services;
using GarageVelo.Services.Mock;
using GarageVelo.ViewModels;
using GarageVelo.Views;

namespace GarageVelo;

public static class MauiProgram
{
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

        // Services (mock implementations — swap for Api/ when backend is ready)
        builder.Services.AddSingleton<ISessionService, MockSessionService>();
        builder.Services.AddSingleton<IAuthService, MockAuthService>();
        builder.Services.AddSingleton<IGarageService, MockGarageService>();
        builder.Services.AddSingleton<ISubscriptionService, MockSubscriptionService>();
        builder.Services.AddSingleton<IPaymentService, MockPaymentService>();
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
