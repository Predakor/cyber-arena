using UnityEngine;

namespace Systems.Shared.Runners
{
    public sealed record VfxEffectOptions(
        bool StickToParent = false,
        bool Loop = false
    );

    public sealed class VfxRunner : Singleton<VfxRunner>
    {
        public static VfxEntity CreateAttatchedEffect(ParticleSystem particles, Transform location, VfxEffectOptions options = null)
        {
            options ??= new VfxEffectOptions() { StickToParent = true };
            return Instance.Run(particles, location, options);
        }

        public static VfxEntity CreateEffect(ParticleSystem particles, Transform location, VfxEffectOptions options = null)
        {
            return Instance.Run(particles, location, options);
        }
        public static VfxEntity CreateEffect(ParticleSystem particles, Vector3 position, Quaternion rotation, VfxEffectOptions options = null)
        {
            return Instance.Run(particles, position, rotation, options);
        }

        public VfxEntity Run(ParticleSystem particles, Transform location, VfxEffectOptions options = null)
        {
            if (particles == null)
            {
                return null;
            }

            return VfxEntity
                .FromParticle(particles)
                .Initialize(location, options)
                .Play();
        }

        public VfxEntity Run(ParticleSystem particles, Vector3 position, Quaternion rotation, VfxEffectOptions options = null)
        {
            if (particles == null)
            {
                return null;
            }

            return VfxEntity
                .FromParticle(particles)
                .Initialize(position, rotation, options)
                .Play();
        }
    }


    public sealed class VfxEntity : MonoBehaviour
    {
        public ParticleSystem Instance { get; private set; }
        private VfxEntity() { }

        public static VfxEntity FromParticle(ParticleSystem particles)
        {
            var instance = Instantiate(particles);

            var entity = instance.gameObject.AddComponent<VfxEntity>();

            entity.Instance = instance;

            var main = instance.main;
            main.stopAction = ParticleSystemStopAction.Callback;

            return entity;
        }

        public VfxEntity Initialize(Transform location, VfxEffectOptions options = null)
        {
            options ??= new VfxEffectOptions();

            Instance.transform.SetPositionAndRotation(location.position, location.rotation);

            if (options.StickToParent)
            {
                Instance.transform.SetParent(location);
                Instance.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            }

            return this;
        }

        public VfxEntity Initialize(Vector3 position, Quaternion rotation, VfxEffectOptions options = null)
        {
            options ??= new VfxEffectOptions();

            Instance.transform.SetPositionAndRotation(position, rotation);

            return this;
        }

        public VfxEntity Play()
        {
            Instance.Play();
            return this;
        }

        public void Stop()
        {
            Instance.Clear();
            Instance.transform.SetParent(null);
            //TODO Add Object pooling
            Destroy(gameObject, 0.01f);
        }

        private void OnParticleSystemStopped()
        {
            Stop();
        }
    }
}
