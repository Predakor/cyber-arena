using UnityEngine;

namespace Assets._Scripts.Utils {
    public static class GameObjectExtensions {
        public static T GetOrAddComponent<T>(this GameObject gameObject)
            where T : Component {
            if (gameObject.TryGetComponent<T>(out var component)) {
                return component;
            }

            return gameObject.AddComponent<T>();
        }

        public static GameObject EnsureComponent<T>(this GameObject go, out T component)
            where T : Component {
            component = go.GetOrAddComponent<T>();
            return go;
        }
    }
}
