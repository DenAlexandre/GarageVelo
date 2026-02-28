using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GarageVelo.Models;
using GarageVelo.Services;

namespace GarageVelo.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
    private readonly IGarageService _garageService;

    public ObservableCollection<Garage> Garages { get; } = [];

    // Default center: Lyon, France
    [ObservableProperty]
    private double _mapLatitude = 45.7578;

    [ObservableProperty]
    private double _mapLongitude = 4.8320;

    public HomeViewModel(IGarageService garageService)
    {
        _garageService = garageService;
        Title = "Carte";
    }

    [RelayCommand]
    private async Task LoadGaragesAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            var garages = await _garageService.GetNearbyAsync(MapLatitude, MapLongitude);

            Garages.Clear();
            foreach (var g in garages)
                Garages.Add(g);
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
    private async Task SelectGarageAsync(Garage garage)
    {
        if (garage is null) return;
        await Shell.Current.GoToAsync($"garageDetail?garageId={garage.Id}");
    }
}
