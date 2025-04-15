using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("音效來源")]
    public AudioSource sfxSource;
    public AudioSource bgmSource;

    [Header("音量控制")]
    [Range(0f, 1f)] public float bgmVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    [System.Serializable]
    public class SoundData
    {
        public string name;
        public AudioClip clip;
    }

    [Header("音效資料庫")]
    public List<SoundData> sounds = new List<SoundData>();
    private Dictionary<string, AudioClip> soundDict;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        soundDict = new Dictionary<string, AudioClip>();
        foreach (var s in sounds)
        {
            if (!soundDict.ContainsKey(s.name))
                soundDict.Add(s.name, s.clip);
        }
    }

    public void PlaySFX(string name)
    {
        if (soundDict.TryGetValue(name, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip, sfxVolume);
        }
        else
        {
            Debug.LogWarning("未找到音效: " + name);
        }
    }

    public void PlayBGM(string name)
    {
        if (soundDict.TryGetValue(name, out AudioClip clip))
        {
            if (bgmSource.clip == clip) return; // 不重複播放同一首
            bgmSource.clip = clip;
            bgmSource.volume = bgmVolume;
            bgmSource.loop = true;
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning("未找到 BGM: " + name);
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }
}