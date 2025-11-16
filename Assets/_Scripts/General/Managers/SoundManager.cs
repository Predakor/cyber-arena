using Assets._Scripts.Utils;
using Systems.Shared;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : Singleton<SoundManager> {
    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    AudioClip[] soundList;

    public static void PlaySound(SoundType sound, float volume) {
        Instance.audioSource.PlayOneShot(Instance.soundList[(int)sound], volume);
    }

    protected override void Awake() {
        base.Awake();
        gameObject.EnsureComponent(out audioSource);
    }
}

public enum SoundType {
    Fire,
    Damage,
    BulletHit,
}
