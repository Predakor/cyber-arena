using Systems.Shared;
using UnityEngine;

namespace Systems._Shared.Runners
{

    public sealed class VfxRunner : Singleton<VfxRunner>
    {
        public VfxEntity Run(ParticleSystem particles, Transform location)
        {
            return VfxEntity
                .FromParticle(particles)
                .Initialize(location)
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

        public VfxEntity Initialize(Transform location)
        {
            Instance.transform.SetPositionAndRotation(location.position, location.rotation);

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

            //TODO Add Object pooling
            Destroy(gameObject);
        }

        private void OnParticleSystemStopped()
        {
            Stop();
        }
    }
}
