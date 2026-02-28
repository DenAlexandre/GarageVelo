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
    public ObservableCollection<Site> Sites { get; } = [];

    [ObservableProperty]
    private Site? _selectedSite;

    public ObservableCollection<Garage> SiteGarages { get; } = [];

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

    partial void OnSelectedSiteChanged(Site? value)
    {
        SiteGarages.Clear();
        if (value is null) return;
        foreach (var g in value.Garages)
            SiteGarages.Add(g);
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

            // Group garages by SiteId to build Sites
            Sites.Clear();
            var grouped = garages.GroupBy(g => g.SiteId);
            foreach (var group in grouped)
            {
                var first = group.First();
                Sites.Add(new Site
                {
                    Id = first.SiteId,
                    Name = first.SiteName,
                    Address = first.Address,
                    Latitude = first.Latitude,
                    Longitude = first.Longitude,
                    Garages = group.ToList()
                });
            }

            // Select first site by default
            if (Sites.Count > 0)
                SelectedSite = Sites[0];
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
    private void SelectSite(Site site)
    {
        if (site is null) return;
        SelectedSite = site;
    }

    [RelayCommand]
    private async Task SelectGarageAsync(Garage garage)
    {
        if (garage is null) return;
        await Shell.Current.GoToAsync($"garageDetail?garageId={garage.Id}");
    }
}
