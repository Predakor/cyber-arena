using System.Collections;
using UnityEngine;

public sealed class IFramesDecorator : IDamageable
{
    private readonly IDamageable _inner;
    private readonly float _duration;//seconds

    private bool hasIFramesActive = false;

    public IFramesDecorator(IDamageable inner, byte invicibilityFramesCount)
    {
        _inner = inner;
        _duration = invicibilityFramesCount;
    }

    public void Damage(int damage, HitOptions options = HitOptions.None)
    {
        if (hasIFramesActive)
        {
            return;
        }

        _inner.Damage(damage, options);
        StartInvincibilityFrames();

    }

    private IEnumerator StartInvincibilityFrames()
    {
        hasIFramesActive = true;
        yield return new WaitForSeconds(_duration);
        hasIFramesActive = false;
    }
}
