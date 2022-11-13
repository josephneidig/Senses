using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager audioManager;

    [SerializeField] private AudioSource _musicSource, _effectsSource;

    void Awake()
    {
        if (audioManager != null && audioManager != this)
        {
            Destroy(this);
        }
        else { audioManager = this; }
    }

    public void PlaySound(AudioClip clip)
    {
        _effectsSource.PlayOneShot(clip);
    }

    public bool isPlaying()
    {
        return _effectsSource.isPlaying;
    }

    public bool exists()
    {
        return true;
    }
}
