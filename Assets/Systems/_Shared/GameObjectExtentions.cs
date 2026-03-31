using UnityEngine;

namespace Assets.Scripts.Utils
{
    public static class GameObjectExtensions
    {
        public static TComponent GetOrAddComponent<TComponent>(this GameObject gameObject)
            where TComponent : Component
        {
            if (gameObject.TryGetComponent<TComponent>(out var component))
            {
                return component;
            }

            return gameObject.AddComponent<TComponent>();
        }

        public static GameObject EnsureComponent<TComponent>(this GameObject go, out TComponent component)
            where TComponent : Component
        {
            component = go.GetOrAddComponent<TComponent>();
            return go;
        }
    }
}
