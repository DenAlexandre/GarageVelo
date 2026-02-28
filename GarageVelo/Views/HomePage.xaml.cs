using GarageVelo.Models;
using GarageVelo.ViewModels;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace GarageVelo.Views;

public partial class HomePage : ContentPage
{
    private readonly HomeViewModel _viewModel;

    public HomePage(HomeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadGaragesCommand.ExecuteAsync(null);
        SetupMap();
    }

    private void SetupMap()
    {
        if (!GarageMap.IsVisible)
            return;

        // Center on Lyon
        GarageMap.MoveToRegion(MapSpan.FromCenterAndRadius(
            new Location(45.7578, 4.832),
            Distance.FromKilometers(3)));

        // Add pins for each site
        GarageMap.Pins.Clear();
        foreach (var site in _viewModel.Sites)
        {
            var availableSlots = site.Garages.Sum(g => g.AvailableSlots);
            var totalSlots = site.Garages.Sum(g => g.TotalSlots);

            var pin = new Pin
            {
                Label = site.Name,
                Address = $"{availableSlots}/{totalSlots} places disponibles",
                Location = new Location(site.Latitude, site.Longitude),
                Type = PinType.Place
            };

            pin.MarkerClicked += (s, e) =>
            {
                e.HideInfoWindow = false;
                _viewModel.SelectSiteCommand.Execute(site);
            };

            GarageMap.Pins.Add(pin);
        }
    }
}
