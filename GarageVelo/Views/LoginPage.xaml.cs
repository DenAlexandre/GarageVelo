using GarageVelo.ViewModels;

namespace GarageVelo.Views;

public partial class LoginPage : ContentPage
{
    private bool _firstAppearing = true;

    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (_firstAppearing)
        {
            _firstAppearing = false;
            EmailEntry.Text = "demo@garagevelo.fr";
            PasswordEntry.Text = "password123";
        }
    }

    private void OnTogglePasswordClicked(object? sender, EventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
    }
}
