using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class SoundManager : Singleton<SoundManager> {


    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] soundList;


    public static void PlaySound(SoundType sound, float volume) {
        Instance.audioSource.PlayOneShot(Instance.soundList[(int)sound], volume);

    }



    protected override void Awake() {
        base.Awake();
        if (audioSource == null) {
            audioSource = GetComponent<AudioSource>();
        }
    }


}


public enum SoundType {
    Fire,
    Damage,
    BulletHit
}