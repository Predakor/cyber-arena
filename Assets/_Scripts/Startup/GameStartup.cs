using Scripts.Projectiles;
using Systems.Guns.Projectiles;
using Systems.Shared.Loggers;
using UnityEngine;

public static class GameStartup
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        GameLogger.Configure(Resources.Load<LogSettings>(GameLogger.SettingsPath));
        ProjectileFactory.Configure(hitHandler: HitHandler.Handle);
    }
}