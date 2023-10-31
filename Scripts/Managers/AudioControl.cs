using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioControl : MonoBehaviour
{
    public bool isMusic;
    public AudioPlayerConfigurator audioConfigurator;
    public AudioSource _audio;

    public float _initVolume;
    public float resumeAudioDelay = 0.1f;

    public void Awake()
    {

        _audio = gameObject.GetComponent<AudioSource>();
        _initVolume = _audio.volume;

        GameManager.Instance.GamePaused += PausePlayAudio;
        GameManager.Instance.GameUnpaused += UnpausePlayAudio;

        if (isMusic)
        {
            GameManager.Instance.MusicVolumeChanged += ChangeSoundVolume;
            ChangeSoundVolume(GameManager.Instance.GiveMusicVolume());
        }
        else
        {
            GameManager.Instance.SoundVolumeChanged += ChangeSoundVolume;
            ChangeSoundVolume(GameManager.Instance.GiveSoundVolume());
        }
    }

    public void StopPlayAudio()
    {
        _audio.Stop();
    }
    public void ResumePlayAudio()
    {
        _audio.Play();
    }
    public void PausePlayAudio()
    {
        _audio.Pause();
    }
    public void UnpausePlayAudio()
    {
        _audio.UnPause();
    }

    public void PlayAudioOneTime(string carrier, string action)
    {
        _audio.Stop();
        AudioClip audioClip = audioConfigurator.GetAudioClip(carrier, action);
        _audio.PlayOneShot(audioClip);
        StartCoroutine(ResumePlayAudio(audioClip.length));
    }

    public void PlayAudioClip(string carrier, string action)
    {
        AudioClip newAudioClip = audioConfigurator.GetAudioClip(carrier, action);
        if (_audio.clip != newAudioClip || !_audio.isPlaying)
        {
            _audio.clip = newAudioClip;
            _audio.Play();
        }
    }
 

    IEnumerator ResumePlayAudio(float waitLength)
    {
        yield return new WaitForSeconds(waitLength + resumeAudioDelay);
        if (!_audio.isPlaying)
            _audio.Play();
    }

    public void ChangeSoundVolume(float value) {
        _audio.volume = _initVolume * value;
    }

    private void OnDestroy()
    {
        if (isMusic)
            GameManager.Instance.MusicVolumeChanged -= ChangeSoundVolume;
        else
            GameManager.Instance.SoundVolumeChanged -= ChangeSoundVolume;
        GameManager.Instance.GamePaused -= PausePlayAudio;
        GameManager.Instance.GameUnpaused -= UnpausePlayAudio;
    }

}
