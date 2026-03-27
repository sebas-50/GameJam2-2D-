using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sources")]
    public AudioSource musicSource;
    public AudioSource ambientSource;
    public AudioSource sfxSource;
    public AudioSource playerSource;
    public AudioSource enemySource;
    public AudioSource emotionSource;
    public AudioSource uiSource;

    [Header("Clips")]
    public List<AudioClip> musicClips;
    public List<AudioClip> ambientClips;
    public List<AudioClip> sfxClips;
    public List<AudioClip> playerClips;
    public List<AudioClip> enemyClips;
    public List<AudioClip> emotionClips;
    public List<AudioClip> uiClips;

    Dictionary<string, AudioClip> musicDict;
    Dictionary<string, AudioClip> ambientDict;
    Dictionary<string, AudioClip> sfxDict;
    Dictionary<string, AudioClip> playerDict;
    Dictionary<string, AudioClip> enemyDict;
    Dictionary<string, AudioClip> emotionDict;
    Dictionary<string, AudioClip> uiDict;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicDict    = LoadDict(musicClips);
        ambientDict  = LoadDict(ambientClips);
        sfxDict      = LoadDict(sfxClips);
        playerDict   = LoadDict(playerClips);
        enemyDict    = LoadDict(enemyClips);
        emotionDict  = LoadDict(emotionClips);
        uiDict       = LoadDict(uiClips);
    }

    Dictionary<string, AudioClip> LoadDict(List<AudioClip> clips)
    {
        var dict = new Dictionary<string, AudioClip>();
        foreach (var clip in clips)
            if (clip != null) dict[clip.name] = clip;
        return dict;
    }

    // -------------------------------------------------------
    //                        MUSIC
    // -------------------------------------------------------
    public void PlayMusic(string name, float fadeTime = 1f)
    {
        if (!musicDict.ContainsKey(name)) return;
        StartCoroutine(FadeInMusic(musicDict[name], fadeTime));
    }

    public void StopMusic(float fadeTime = 1f)
    {
        StartCoroutine(FadeOutMusic(fadeTime));
    }

    IEnumerator FadeInMusic(AudioClip newClip, float time)
    {
        float targetVol = musicSource.volume > 0 ? musicSource.volume : 1f;

        if (musicSource.isPlaying)
            yield return StartCoroutine(FadeOutMusic(time));

        musicSource.clip = newClip;
        musicSource.loop = true;
        musicSource.Play();

        float t = 0;
        while (t < time)
        {
            musicSource.volume = Mathf.Lerp(0, targetVol, t / time);
            t += Time.deltaTime;
            yield return null;
        }
        musicSource.volume = targetVol;
    }

    IEnumerator FadeOutMusic(float time)
    {
        float startVol = musicSource.volume;
        float t = 0;
        while (t < time)
        {
            musicSource.volume = Mathf.Lerp(startVol, 0, t / time);
            t += Time.deltaTime;
            yield return null;
        }
        musicSource.volume = 0;
        musicSource.Stop();
    }

    // -------------------------------------------------------
    //                      CATEGORY SFX
    // -------------------------------------------------------
    public void PlaySFX(string name)
        => PlayFromDict(sfxDict, sfxSource, name);

    public void PlayPlayerSFX(string name)
        => PlayFromDict(playerDict, playerSource, name);

    public void PlayEnemySFX(string name)
        => PlayFromDict(enemyDict, enemySource, name);

    public void PlayEmotion(string name)
        => PlayFromDict(emotionDict, emotionSource, name);

    public void PlayUI(string name)
        => PlayFromDict(uiDict, uiSource, name);

    public void PlayAmbience(string name)
    {
        if (!ambientDict.ContainsKey(name)) return;
        ambientSource.clip = ambientDict[name];
        ambientSource.loop = true;
        ambientSource.Play();
    }

    public void StopAmbience() => ambientSource.Stop();

    void PlayFromDict(Dictionary<string, AudioClip> dict, AudioSource source, string name)
    {
        if (dict.ContainsKey(name))
            source.PlayOneShot(dict[name]);
    }
}