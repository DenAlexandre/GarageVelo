using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GarageVelo.Models;
using GarageVelo.Services;

namespace GarageVelo.ViewModels;

public partial class ProfileViewModel : BaseViewModel
{
    private readonly IAuthService _authService;
    private readonly ISubscriptionService _subscriptionService;

    [ObservableProperty]
    private User? _user;

    [ObservableProperty]
    private Subscription? _activeSubscription;

    [ObservableProperty]
    private bool _hasActiveSubscription;

    public ProfileViewModel(IAuthService authService, ISubscriptionService subscriptionService)
    {
        _authService = authService;
        _subscriptionService = subscriptionService;
        Title = "Profil";
    }

    [RelayCommand]
    private async Task LoadProfileAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;

            User = await _authService.GetCurrentUserAsync();
            if (User is not null)
            {
                ActiveSubscription = await _subscriptionService.GetActiveSubscriptionAsync(User.Id);
                HasActiveSubscription = ActiveSubscription is not null;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Erreur: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        await _authService.LogoutAsync();
        await Shell.Current.GoToAsync("//login");
    }
}
