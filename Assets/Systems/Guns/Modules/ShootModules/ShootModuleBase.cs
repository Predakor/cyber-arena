using Systems.Guns.Utils;
using UnityEngine;

namespace Systems.Guns.Modules.ShootModules {
    public interface IShootHandler {
        void Pressed();
        void Released();
    }

    public abstract class ShootModuleBase : MonoBehaviour, IShootHandler {
        protected FireRateController fireRateController;

        [SerializeField]
        protected short roundPerMinute;

        protected virtual void Awake() {
            fireRateController = FireRateController.FromRPM(roundPerMinute);
        }

        public abstract void Pressed();

        public abstract void Released();
    }
}
