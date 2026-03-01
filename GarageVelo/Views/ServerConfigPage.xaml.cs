namespace GarageVelo.Views;

public partial class ServerConfigPage : ContentPage
{
    private static readonly string[] PredefinedUrls =
    [
        "http://192.168.1.36:5000",
        "http://localhost:5000",
        "http://localhost:6000",
        "https://localhost:5001",
        "http://10.0.2.2:5000",   // Android emulator
        "http://10.0.2.2:6000",
    ];

    private const string CustomLabel = "Personnalise...";

    /// <summary>Fires when the user successfully connected.</summary>
    public event Action? ConnectedSuccessfully;

    public void NotifyConnected() => ConnectedSuccessfully?.Invoke();

    public ServerConfigPage()
    {
        InitializeComponent();

        // Fill picker
        foreach (var url in PredefinedUrls)
            ServerPicker.Items.Add(url);
        ServerPicker.Items.Add(CustomLabel);

        // Select current saved URL
        var saved = Preferences.Get("api_base_url", PredefinedUrls[0]);
        var idx = Array.IndexOf(PredefinedUrls, saved);
        if (idx >= 0)
        {
            ServerPicker.SelectedIndex = idx;
        }
        else
        {
            // Custom URL previously saved
            ServerPicker.SelectedIndex = ServerPicker.Items.Count - 1;
            CustomUrlEntry.Text = saved;
            CustomUrlEntry.IsVisible = true;
        }
    }

    private void OnServerPickerChanged(object? sender, EventArgs e)
    {
        var isCustom = ServerPicker.SelectedIndex == ServerPicker.Items.Count - 1;
        CustomUrlEntry.IsVisible = isCustom;
        StatusLabel.IsVisible = false;
    }

    private async void OnRetryClicked(object? sender, EventArgs e)
    {
        var url = GetSelectedUrl();
        if (string.IsNullOrWhiteSpace(url))
        {
            ShowStatus("Veuillez saisir une adresse valide.", true);
            return;
        }

        // Save selection
        Preferences.Set("api_base_url", url);

        // Test connection
        RetryButton.IsEnabled = false;
        RetryButton.Text = "Test en cours...";
        ShowStatus($"Connexion a {url}...", false);

        try
        {
            using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
            await client.GetAsync($"{url}/swagger/index.html");

            ShowStatus("Connexion reussie !", false);
            await Task.Delay(500);
            ConnectedSuccessfully?.Invoke();
        }
        catch
        {
            ShowStatus($"Echec de connexion a {url}", true);
        }
        finally
        {
            RetryButton.IsEnabled = true;
            RetryButton.Text = "Tester et continuer";
        }
    }

    private void OnQuitClicked(object? sender, EventArgs e)
    {
        Application.Current?.Quit();
    }

    private string GetSelectedUrl()
    {
        if (ServerPicker.SelectedIndex < 0)
            return string.Empty;

        if (ServerPicker.SelectedIndex == ServerPicker.Items.Count - 1)
            return CustomUrlEntry.Text?.Trim() ?? string.Empty;

        return PredefinedUrls[ServerPicker.SelectedIndex];
    }

    private void ShowStatus(string message, bool isError)
    {
        StatusLabel.Text = message;
        StatusLabel.TextColor = isError
            ? (Color)Application.Current!.Resources["Error"]
            : (Color)Application.Current!.Resources["Success"];
        StatusLabel.IsVisible = true;
    }
}
