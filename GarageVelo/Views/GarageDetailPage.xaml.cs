using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using GarageVelo.ViewModels;

namespace GarageVelo.Views;

public partial class GarageDetailPage : ContentPage
{
    private readonly GarageDetailViewModel _viewModel;

    public GarageDetailPage(GarageDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateMap();
    }

    private void UpdateMap()
    {
        var garage = _viewModel.Garage;
        if (garage is null) return;

        var location = new Location(garage.Latitude, garage.Longitude);
        DetailMap.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(0.5)));
        DetailMap.Pins.Clear();
        DetailMap.Pins.Add(new Pin
        {
            Label = garage.Name,
            Address = garage.Address,
            Type = PinType.Place,
            Location = location
        });
    }

    // Re-update map when garage data loads
    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        if (_viewModel?.Garage is not null)
            UpdateMap();
    }
}
