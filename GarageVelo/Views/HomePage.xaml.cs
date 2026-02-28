using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using GarageVelo.Models;
using GarageVelo.ViewModels;

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

        // Center map on Lyon
        var lyon = new Location(45.7578, 4.8320);
        GarageMap.MoveToRegion(MapSpan.FromCenterAndRadius(lyon, Distance.FromKilometers(3)));

        await _viewModel.LoadGaragesCommand.ExecuteAsync(null);
        AddPinsToMap();
    }

    private void AddPinsToMap()
    {
        GarageMap.Pins.Clear();
        foreach (var garage in _viewModel.Garages)
        {
            var pin = new Pin
            {
                Label = garage.Name,
                Address = $"{garage.AvailableSlots} places disponibles",
                Type = PinType.Place,
                Location = new Location(garage.Latitude, garage.Longitude)
            };
            pin.InfoWindowClicked += async (s, e) =>
            {
                await _viewModel.SelectGarageCommand.ExecuteAsync(garage);
            };
            GarageMap.Pins.Add(pin);
        }
    }
}
