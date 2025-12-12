using System;

public static class BuildingPower
{
    public static bool IsOn { get; private set; } = false;
    public static event Action<bool> Changed;

    public static void SetPower(bool on)
    {
        if (IsOn == on) return;
        IsOn = on;
        Changed?.Invoke(IsOn);
    }
}