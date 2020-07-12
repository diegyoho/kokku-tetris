using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utilities;

public class SoundController :
    SingletonMonoBehaviour<SoundController> {

    AudioSource musicSource;
    AudioSource sfxSource;

    bool _musicMuted;
    bool _soundMuted;
    public static bool musicMuted {
        get { return instance._musicMuted; }
        set {
            instance._musicMuted = value;
            instance.musicSource.mute = value;
        }
    }
    public static bool soundMuted {
        get { return instance._soundMuted; }
        set {
            instance._soundMuted = value;
            instance.sfxSource.mute = value;
        }
    }

    public override void Awake() {
        base.Awake();

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        sfxSource = gameObject.AddComponent<AudioSource>();
    }

    public static void PlayMusic(AudioClip clip, float volume = 1f) {
        if (instance.musicSource.clip == clip)
            return;
        
        instance.musicSource.clip = clip;
        instance.musicSource.volume = volume;
        instance.musicSource.Play();
    }

    public static void PlayMusicFade(AudioClip clip, float volume = 1f) {
        if (instance.musicSource.clip == clip)
            return;
            
        instance.StartCoroutine(instance.IEPlayMusicFade(clip, volume));
    }

    IEnumerator IEPlayMusicFade(AudioClip clip, float volume) {
        float t = 1, lastVolume = musicSource.volume;

        if (musicSource.isPlaying) {
            while (t > 0) {
                musicSource.volume = Mathf.Lerp(0, lastVolume, t);
                t -= 0.1f;
                yield return new WaitForSecondsRealtime(0.1f);
            }

            musicSource.Stop();
        }

        musicSource.clip = clip;
        musicSource.volume = 0;
        musicSource.Play();
        t = 0;

        while (t < volume) {
            musicSource.volume = Mathf.Lerp(0, volume, t);
            t += 0.1f;
            yield return new WaitForSecondsRealtime(0.1f);
        }

        musicSource.volume = volume;
    }

    public static void PlaySfx(AudioClip clip, float volume = 1f) {
        instance.sfxSource.clip = clip;
        instance.sfxSource.volume = volume;
        instance.sfxSource.Play();
    }

    public void PlaySfxNonStatic(AudioClip clip) {
        instance.sfxSource.clip = clip;
        instance.sfxSource.volume = 1f;
        instance.sfxSource.Play();
    }

    public static AudioSource PlaySfxInstance(AudioClip clip, float volume = 1f) {
        AudioSource sfxSource = Instantiate(
            Resources.Load<GameObject>("Prefabs/sfxSourcePrefab")
        ).GetComponent<AudioSource>();
        
        sfxSource.mute = soundMuted;
        sfxSource.clip = clip;
        sfxSource.volume = volume;
        sfxSource.Play();
        return sfxSource;
    }

    public static void StopMusic() {
        instance.musicSource.Stop();
    }

    public static void FadeOut(AudioSource asrc) {
        instance.StartCoroutine(instance.FadeOutEnum(asrc));
    }

    IEnumerator FadeOutEnum(AudioSource asrc) {
        float t = 1, lastVolume = asrc.volume;
        if (asrc.isPlaying) {
            while (t > 0) {
                asrc.volume = Mathf.Lerp(0, lastVolume, t);
                t -= 0.1f;
                yield return new WaitForSecondsRealtime(0.1f);
            }

            asrc.Stop();
        }
    }
}
