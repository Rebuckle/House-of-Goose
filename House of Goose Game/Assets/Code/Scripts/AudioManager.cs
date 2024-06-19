using UnityEngine;
using DG.Tweening;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip titleTheme;
    [SerializeField] private AudioClip mainTheme;
    private AudioSource audioSrc;
    public AudioSource AudioSrc
    {
        get
        {
            if (audioSrc == null)
            {
                audioSrc = GetComponent<AudioSource>();
                if (audioSrc == null)
                {
                    Debug.LogWarning("No AudioSource found on AudioManager.");
                    return null;
                }
            }
            return audioSrc;
        }
    }

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        AudioSrc.clip = titleTheme;
        AudioSrc.Play();
    }

    public void FadeAudioToVolume(float volume, float duration = 1.5f)
    {
        AudioSrc.DOFade(volume, duration);
    }

    public void FadeAudioBetween(float duration)
    {
        float ogVol = AudioSrc.volume;
        Tween themeTween = AudioSrc.DOFade(0, duration);
        themeTween.OnComplete(() => SetAudioMain(duration));
    }

    private void SetAudioMain(float duration)
    {
        AudioSrc.clip = mainTheme;
        AudioSrc.Play();
        AudioSrc.DOFade(0.35f, duration);
    }
}
