using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Helpers.Collections {

    public class CollectionHelpers : MonoBehaviour {

        T GetElement<T>(IEnumerable<T> collection, int start, int end) {
            int rand = Random.Range(start, end);
            return collection.ElementAt(rand);
        }

        public static T RandomElement<T>(List<T> collection, int start = 0) {
            if (collection.Count == 1) return collection[0];
            start = Mathf.Clamp(start, 0, collection.Count);
            return collection[Random.Range(start, collection.Count)];
        }
        public static T RandomElement<T>(T[] collection, int start = 0) {
            if (collection.Length == 1) return collection[0];
            start = Mathf.Clamp(start, 0, collection.Length);
            return collection[Random.Range(start, collection.Length)];
        }

    }
}
