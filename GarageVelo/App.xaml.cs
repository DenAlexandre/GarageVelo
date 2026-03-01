using GarageVelo.Services;
using GarageVelo.Views;

namespace GarageVelo;

public partial class App : Application
{
    private readonly ISessionService _sessionService;
    private readonly IHttpClientFactory? _httpClientFactory;

    public App(ISessionService sessionService, IHttpClientFactory? httpClientFactory = null)
    {
        InitializeComponent();
        _sessionService = sessionService;
        _httpClientFactory = httpClientFactory;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        if (_httpClientFactory is null)
        {
            // Mock mode: no server check needed
            return new Window(new AppShell());
        }

        // Start on the server config page — test connectivity first
        var configPage = new ServerConfigPage();
        var window = new Window(new NavigationPage(configPage));

        configPage.ConnectedSuccessfully += async () =>
        {
            // Server is reachable — switch to the real app
            var shell = new AppShell();
            window.Page = shell;

            // Small delay to let Shell initialize
            await Task.Delay(100);

            var isAuthenticated = await _sessionService.IsAuthenticatedAsync();
            if (!isAuthenticated)
            {
                await Shell.Current.GoToAsync("//login");
            }
        };

        // Auto-test on load
        configPage.Loaded += async (_, _) =>
        {
            await Task.Delay(200);
            await AutoTestServerAsync(configPage);
        };

        return window;
    }

    private static async Task AutoTestServerAsync(ServerConfigPage configPage)
    {
        var url = Preferences.Get("api_base_url", "http://192.168.1.36:5000");
        try
        {
            using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
            await client.GetAsync($"{url}/swagger/index.html");

            // Server is reachable — skip config page
            configPage.NotifyConnected();
        }
        catch
        {
            // Server not reachable — user stays on config page
        }
    }
}
