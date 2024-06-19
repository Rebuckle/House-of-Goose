using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXHandler : MonoBehaviour
{
    private AudioSource asource;

    public void Awake()
    {
        asource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        asource.PlayOneShot(clip);
    }

    public void PlaySoundLooping(AudioClip clip)
    {
        asource.clip = clip;
        asource.loop = true;
        asource.Play();
    }
}
