using Scripts.Projectiles;
using Systems.Guns.Projectiles;
using UnityEngine;

public static class GameStartup
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        ProjectileFactory.Configure(hitHandler: HitHandler.Handle);
    }
}