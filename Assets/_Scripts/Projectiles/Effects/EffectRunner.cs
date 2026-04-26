using System;
using System.Collections;
using System.Collections.Generic;
using Systems.Guns.HitEffects;
using Systems.Guns.Projectiles;
using Systems.Shared;
using UnityEngine;

namespace Systems.Guns.HitEffect
{
    public sealed class EffectRunner : Singleton<EffectRunner>
    {
        private static readonly EffectsCollection Active = new();

        public EffectHandler<TEffect> From<TEffect>(TEffect effect, HitInfo hit)
            where TEffect : IHitEffect
        {
            var handler = new EffectHandler<TEffect>(effect, hit, this);
            return Active.Add(handler);

        }

        public EffectHandler<TEffect> StartEffect<TEffect>(TEffect effect, HitInfo hit)
            where TEffect : IHitEffect
        {
            var handler = new EffectHandler<TEffect>(effect, hit, this);

            return Active.Add(handler)
                .Apply()
                .ClearAfter();
        }

        public void Cancel<TEffect>(EffectHandler<TEffect> effect)
            where TEffect : IHitEffect
        {
            Active.Remove(effect);
        }

        public void ClearAll() => Active.ClearAll();

        internal Coroutine RunCoroutine(IEnumerator coroutine) => StartCoroutine(coroutine);

        internal void EndCoroutine(Coroutine coroutine) => StopCoroutine(coroutine);
    }

    sealed class EffectsCollection
    {
        private readonly Dictionary<Type, List<IEffectHandler>> _activeEffects = new();

        public int AllEffectCount()
        {
            var totalCount = 0;
            foreach (var effect in _activeEffects)
            {
                totalCount += effect.Value.Count;
            }
            return totalCount;
        }

        public int EffectsOfTypeCounf<TEffect>()
            where TEffect : IHitEffect
        {
            GetEffectGroup<TEffect>(out var x);
            return x?.Count ?? 0;
        }

        public EffectHandler<TEffect> Add<TEffect>(EffectHandler<TEffect> handler)
            where TEffect : IHitEffect
        {
            var effect = handler as IEffectHandler;

            if (GetEffectGroup<TEffect>(out List<IEffectHandler> effects))
            {
                effects.Add(effect);
                return handler;
            }

            effects = new List<IEffectHandler> { effect };
            _activeEffects.Add(typeof(TEffect), effects);
            return handler;
        }

        public void Remove<TEffect>(EffectHandler<TEffect> handler)
            where TEffect : IHitEffect
        {
            if (!GetEffectGroup<TEffect>(out List<IEffectHandler> effects))
            {
                return;
            }

            var effect = handler as IEffectHandler;

            if (effects.Remove(effect))
            {
                effect.Clear();
            }
        }

        public void ClearAll()
        {
            foreach (var effectGroups in _activeEffects)
            {
                var effectList = effectGroups.Value;
                ClearGroup(effectList);
            }
        }

        public void ClearGroup<TEffect>(TEffect effect)
            where TEffect : IHitEffect
        {
            if (!GetEffectGroup<TEffect>(out List<IEffectHandler> effects))
            {
                return;
            }

            ClearGroup(effects);
        }

        private bool GetEffectGroup<TEffect>(out List<IEffectHandler> effects)
            where TEffect : IHitEffect
        {
            return _activeEffects.TryGetValue(typeof(TEffect), out effects);
        }

        private void ClearGroup(List<IEffectHandler> effectList)
        {
            foreach (var effect in effectList)
            {
                effect.Clear();
            }
            effectList.Clear();
        }
    }
}
