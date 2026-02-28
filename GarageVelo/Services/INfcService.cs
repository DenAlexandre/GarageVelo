namespace GarageVelo.Services;

/// <summary>
/// NFC service placeholder for phase 2 (cylinder programming).
/// </summary>
public interface INfcService
{
    Task<bool> IsNfcAvailableAsync();
    Task<bool> WriteLockCodeAsync(string lockCode);
}
