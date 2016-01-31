using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : Singleton<MusicPlayer>
{
    const float MASTER_VOLUME = 1f;

    readonly static Dictionary<string, float> VOLUMES = new Dictionary<string, float> {
        {"TitleScreen", 2f},
        {"Puzzle1", .4f}
    };

    Dictionary<string, AudioClip> _clips;
    AudioSource _source;

    void Awake()
    {
        if (KeepAcrossScenes()) return;

        _clips = new Dictionary<string, AudioClip>();
        _source = gameObject.AddComponent<AudioSource>();
        _source.volume = MASTER_VOLUME;
    }

    public void PlayTrack(string name, bool loop)
    {
        AudioClip clip;
        float volume = 1f;

        if (_clips.ContainsKey(name)) {
            clip = _clips[name];
        } else {
            _clips[name] = Resources.Load("Music/"+name) as AudioClip;
            clip = _clips[name];
        }

        if (VOLUMES.ContainsKey(name)) {
            volume = VOLUMES[name];
        }

        _source.loop = loop;
        _source.clip = clip;
        _source.volume = MASTER_VOLUME * volume;
        _source.Play();
    }

    public void Stop()
    {
        _source.Stop();
    }
}
