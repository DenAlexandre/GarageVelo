using GarageVelo.Views;

namespace GarageVelo;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Register routes for push navigation (not in TabBar)
        Routing.RegisterRoute("register", typeof(RegisterPage));
        Routing.RegisterRoute("garageDetail", typeof(GarageDetailPage));
        Routing.RegisterRoute("subscription", typeof(SubscriptionPage));
    }
}
