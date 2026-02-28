using GarageVelo.ViewModels;

namespace GarageVelo.Views;

public partial class GarageDetailPage : ContentPage
{
    public GarageDetailPage(GarageDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
