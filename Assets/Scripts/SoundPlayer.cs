using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : Singleton<SoundPlayer>
{
    const float MASTER_VOLUME = .4f;

    readonly static Dictionary<string, float> VOLUMES = new Dictionary<string, float> {
    };

    Dictionary<string, AudioClip[]> _clipSets;
    AudioSource _sounder;

    void Awake()
    {
        if (KeepAcrossScenes()) return;

        _clipSets = new Dictionary<string, AudioClip[]>();

        _sounder = gameObject.AddComponent<AudioSource>();
        _sounder.volume = MASTER_VOLUME;
    }

    public void Play(string name)
    {
        AudioClip clip;
        float volume = 1f;

        if (_clipSets.ContainsKey(name)) {
            clip = sampleClipSet(_clipSets[name]);
        } else {
            _clipSets[name] = loadFolder("Audio/"+name);
            clip = sampleClipSet(_clipSets[name]);
        }

        if (VOLUMES.ContainsKey(name)) {
            volume = VOLUMES[name];
        }

        _sounder.PlayOneShot(clip, volume);
    }

    static AudioClip sampleClipSet(AudioClip[] clipset)
    {
        return clipset[Mathf.FloorToInt(clipset.Length * Random.value)];
    }

    static AudioClip[] loadFolder(string folderName)
    {
        var objects = Resources.LoadAll(folderName);
        var list = new List<AudioClip>();

        foreach (var o in objects) {
            list.Add(o as AudioClip);
        }

        return list.ToArray();
    }
}

