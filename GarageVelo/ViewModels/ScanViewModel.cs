using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GarageVelo.Services;

namespace GarageVelo.ViewModels;

public partial class ScanViewModel : BaseViewModel
{
    private readonly IGarageService _garageService;

    [ObservableProperty]
    private bool _isScanning = true;

    [ObservableProperty]
    private string _scanResult = string.Empty;

    public ScanViewModel(IGarageService garageService)
    {
        _garageService = garageService;
        Title = "Scanner QR";
    }

    [RelayCommand]
    private async Task ProcessQrCodeAsync(string qrPayload)
    {
        if (string.IsNullOrWhiteSpace(qrPayload) || IsBusy) return;

        try
        {
            IsBusy = true;
            IsScanning = false;
            ScanResult = qrPayload;

            var garage = await _garageService.GetByQrCodeAsync(qrPayload);
            if (garage is null)
            {
                ErrorMessage = "Garage non trouvé.";
                IsScanning = true;
                return;
            }

            await Shell.Current.GoToAsync($"garageDetail?garageId={garage.Id}");
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Erreur: {ex.Message}";
            IsScanning = true;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void ResetScanner()
    {
        IsScanning = true;
        ScanResult = string.Empty;
        ErrorMessage = string.Empty;
    }
}
