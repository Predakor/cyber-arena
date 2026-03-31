namespace Systems.Channels.Weapons
{
    public static class WeaponEvents
    {
        public sealed record Selected(byte Slot, byte? PrevSlot);
        public sealed record Fired;
        public sealed record AmmoChanged(int Current, int? Reserve = null);
        public sealed record ReloadStarted(float Duration);
        public sealed record ReloadFinished(float Duration, bool Interupted = false);
    }
}
