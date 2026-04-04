using System.Collections;
using UnityEngine;

namespace Systems.Shared
{
    public sealed class CoroutineRunner : Singleton<CoroutineRunner>
    {
        public static Coroutine Run(IEnumerator routine) => Instance.StartCoroutine(routine);

        public static void Stop(Coroutine coroutine) => Instance.StopCoroutine(coroutine);
    }
}