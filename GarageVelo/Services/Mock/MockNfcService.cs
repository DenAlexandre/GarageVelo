namespace GarageVelo.Services.Mock;

/// <summary>
/// Phase 2 placeholder: NFC cylinder programming.
/// </summary>
public class MockNfcService : INfcService
{
    public Task<bool> IsNfcAvailableAsync()
    {
        return Task.FromResult(false);
    }

    public Task<bool> WriteLockCodeAsync(string lockCode)
    {
        // Not implemented in phase 1
        return Task.FromResult(false);
    }
}
