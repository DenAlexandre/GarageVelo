using GarageVelo.ViewModels;

namespace GarageVelo.Views;

public partial class SubscriptionPage : ContentPage
{
    public SubscriptionPage(SubscriptionViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
