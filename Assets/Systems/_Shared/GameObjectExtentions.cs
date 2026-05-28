using UnityEngine;

namespace Assets.Scripts.Utils
{
    public static class GameObjectExtensions
    {
        public static TComponent GetOrAddComponent<TComponent>(this GameObject gameObject)
            where TComponent : Component
        {
            return gameObject.TryGetComponent<TComponent>(out var component)
                ? component
                : gameObject.AddComponent<TComponent>();

        }

        public static GameObject EnsureComponent<TComponent>(this GameObject go, out TComponent component)
            where TComponent : Component
        {
            component = go.GetOrAddComponent<TComponent>();
            Debug.Assert(component != null, "Component not found and adding failed", go);
            return go;
        }
    }
}
