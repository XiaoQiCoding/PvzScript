using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static SoundManager instance;
    // 用于播放音乐
    private AudioSource audioSource;
    // 
    private Dictionary<string, AudioClip> dictAudio;
    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
        dictAudio = new Dictionary<string, AudioClip>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // 辅助函数： 加载音频，需要确保音频文件的路径在Resources文件夹下
    public AudioClip LoadAudio(string path)
    {
        return (AudioClip)Resources.Load(path);
    }

    // 辅助函数：获取音频，并且将其缓存在dictAudio中，避免重复加载
    private AudioClip GetAudio(string path)
    {
        if(!dictAudio.ContainsKey(path))
        {
            dictAudio[path] = LoadAudio(path);
        }
        return dictAudio[path];
    }

    public void PlayBGM(string name, float volume = 1.0f)
    {
        audioSource.Stop();
        audioSource.clip = GetAudio(name);
        audioSource.Play();
    }

    public void StopBGM()
    {
        audioSource.Stop();
    }

    // 播放音效
    public void PlaySound(string path, float volume = 1f)
    {
        // PlayOneShot可以叠加播放
        this.audioSource.PlayOneShot(GetAudio(path), volume);
        // this.audioSource.volume = volume;
    }

    public void PlaySound(AudioSource audioSource, string path, float volume=1f)
    {
        audioSource.PlayOneShot(GetAudio(path), volume);
        // audioSource.volume = volume;
    }
}
