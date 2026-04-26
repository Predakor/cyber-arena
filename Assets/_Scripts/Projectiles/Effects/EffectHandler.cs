using System.Collections;
using Systems.Guns.HitEffects;
using Systems.Guns.Projectiles;
using UnityEngine;

namespace Systems.Guns.HitEffect {
    public interface IEffectHandler {
        IEffectHandler Apply();
        IEffectHandler Clear();
    }

    public sealed class EffectHandler<TEffect>
        where TEffect : IHitEffect {
        private readonly TEffect _effect;
        private readonly HitInfo _hit;
        private readonly EffectRunner _runner;

        private Coroutine _coroutine;

        internal EffectHandler(TEffect effect, HitInfo hit, EffectRunner runner) {
            _effect = effect;
            _hit = hit;
            _runner = runner;
        }

        public EffectHandler<TEffect> Apply() {
            _effect.Apply(_hit);
            return this;
        }

        public EffectHandler<TEffect> ClearAfter(float? seconds = null) {
            _coroutine = _runner.RunCoroutine(ClearCoroutine(seconds));
            return this;
        }

        private IEnumerator ClearCoroutine(float? seconds) {
            yield return new WaitForSeconds(seconds ?? _effect.Duration);
            _runner.EndCoroutine(_coroutine);
            _effect?.Clear(_hit);
        }
    }
}
