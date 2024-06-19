using UnityEngine;

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
}
