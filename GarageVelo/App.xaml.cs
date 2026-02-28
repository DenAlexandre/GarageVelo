using GarageVelo.Services;
using GarageVelo.Views;

namespace GarageVelo;

public partial class App : Application
{
    private readonly ISessionService _sessionService;

    public App(ISessionService sessionService)
    {
        InitializeComponent();
        _sessionService = sessionService;
    }

    protected override async void OnStart()
    {
        base.OnStart();

        var isAuthenticated = await _sessionService.IsAuthenticatedAsync();
        if (!isAuthenticated)
        {
            await Shell.Current.GoToAsync($"//login");
        }
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}
