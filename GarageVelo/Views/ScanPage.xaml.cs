using GarageVelo.ViewModels;

namespace GarageVelo.Views;

public partial class ScanPage : ContentPage
{
    public ScanPage(ScanViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        // Set demo QR payload via code-behind (JSON braces break XAML markup)
        DemoScanButton.CommandParameter = "{\"id\":\"GV-0001\",\"pos\":7,\"lock\":\"814623\",\"size\":\"Medium\"}";
    }
}
