using System.Collections.Generic;
using UnityEngine;


namespace Helpers.Collections {

    public static class CollectionUtils {
        public static T RandomElement<T>(List<T> collection, int start = 0) {
            return GetElement(collection, start, collection?.Count ?? 0);
        }

        public static T RandomElement<T>(T[] collection, int start = 0) {
            return GetElement(collection, start, collection?.Length ?? 0);
        }

        private static T GetElement<T>(IList<T> collection, int start, int end) {
            if (end == 0) {
                Debug.LogError($"Collection is null or empty: {typeof(T).Name}");
                return default;
            }

            start = Mathf.Clamp(start, 0, end - 1);
            end = Mathf.Clamp(end, start + 1, end);

            return collection[Random.Range(start, end)];
        }
    }
}
