using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GarageVelo.Models;
using GarageVelo.Services;

namespace GarageVelo.ViewModels;

[QueryProperty(nameof(GarageId), "garageId")]
public partial class SubscriptionViewModel : BaseViewModel
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly IPaymentService _paymentService;
    private readonly IAuthService _authService;

    [ObservableProperty]
    private string _garageId = string.Empty;

    [ObservableProperty]
    private SubscriptionPlan? _selectedPlan;

    [ObservableProperty]
    private bool _isProcessingPayment;

    [ObservableProperty]
    private bool _paymentSuccess;

    [ObservableProperty]
    private string _successMessage = string.Empty;

    public ObservableCollection<SubscriptionPlan> Plans { get; } = [];

    public SubscriptionViewModel(
        ISubscriptionService subscriptionService,
        IPaymentService paymentService,
        IAuthService authService)
    {
        _subscriptionService = subscriptionService;
        _paymentService = paymentService;
        _authService = authService;
        Title = "Abonnement";
    }

    partial void OnGarageIdChanged(string value)
    {
        if (!string.IsNullOrEmpty(value))
            LoadPlansCommand.ExecuteAsync(null);
    }

    [RelayCommand]
    private async Task LoadPlansAsync()
    {
        try
        {
            IsBusy = true;
            var plans = await _subscriptionService.GetPlansAsync();
            Plans.Clear();
            foreach (var plan in plans)
                Plans.Add(plan);
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
    private void SelectPlan(SubscriptionPlan plan)
    {
        SelectedPlan = plan;
    }

    [RelayCommand]
    private async Task ConfirmPaymentAsync()
    {
        if (SelectedPlan is null)
        {
            ErrorMessage = "Veuillez sélectionner un plan.";
            return;
        }

        try
        {
            IsProcessingPayment = true;
            ErrorMessage = string.Empty;

            var user = await _authService.GetCurrentUserAsync();
            if (user is null)
            {
                ErrorMessage = "Session expirée. Veuillez vous reconnecter.";
                return;
            }

            // Process payment
            var payment = await _paymentService.ProcessPaymentAsync(user.Id, SelectedPlan.Price);
            if (payment.Status != PaymentStatus.Completed)
            {
                ErrorMessage = "Le paiement a échoué. Veuillez réessayer.";
                return;
            }

            // Activate subscription
            var subscription = await _subscriptionService.ActivateAsync(user.Id, GarageId, SelectedPlan.Type);
            if (subscription is null)
            {
                ErrorMessage = "Erreur lors de l'activation de l'abonnement.";
                return;
            }

            PaymentSuccess = true;
            SuccessMessage = $"Abonnement {SelectedPlan.Name} activé jusqu'au {subscription.EndDate:dd/MM/yyyy}";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Erreur: {ex.Message}";
        }
        finally
        {
            IsProcessingPayment = false;
        }
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
