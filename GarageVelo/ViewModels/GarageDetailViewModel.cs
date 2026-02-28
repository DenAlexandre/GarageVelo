using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GarageVelo.Models;
using GarageVelo.Services;

namespace GarageVelo.ViewModels;

[QueryProperty(nameof(GarageId), "garageId")]
public partial class GarageDetailViewModel : BaseViewModel
{
    private readonly IGarageService _garageService;

    [ObservableProperty]
    private string _garageId = string.Empty;

    [ObservableProperty]
    private Garage? _garage;

    public GarageDetailViewModel(IGarageService garageService)
    {
        _garageService = garageService;
        Title = "Détail Garage";
    }

    partial void OnGarageIdChanged(string value)
    {
        if (!string.IsNullOrEmpty(value))
            LoadGarageCommand.ExecuteAsync(null);
    }

    [RelayCommand]
    private async Task LoadGarageAsync()
    {
        if (string.IsNullOrEmpty(GarageId)) return;

        try
        {
            IsBusy = true;
            Garage = await _garageService.GetByIdAsync(GarageId);
            if (Garage is not null)
                Title = Garage.Name;
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
    private async Task GoToSubscriptionAsync()
    {
        if (Garage is null) return;
        await Shell.Current.GoToAsync($"subscription?garageId={Garage.Id}");
    }
}
