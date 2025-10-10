using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    AudioSource audioSource;

    [SerializeField] List<AudioSource> audioSources;

    protected override void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        IsOpenSound = LocalStore.GetSound();
        OpenSound(IsOpenSound);
        base.Awake();
    }

    private void Start()
    {
        audioSources = FindObjectsOfType<AudioSource>().ToList();
    }

    public void PlaySound(string name, bool loop = false)
    {
        AudioClip clip = ResourcesCache.Load<AudioClip>(@"Sounds/" + name);
        if (clip != null)
        {
            if (loop)
            {
                audioSource.clip = clip;
                audioSource.loop = true;
                audioSource.Play();
            }
            else
            {
                audioSource.PlayOneShot(clip);
            }

        }
    }
    public void StopSound()
    {
        audioSource.clip = null;
        audioSource.loop = false;
        if (audioSource.isPlaying)
            audioSource.Stop();
    }
    public void OpenSound(bool IsOpen)
    {
        audioSource.volume = IsOpen ? 1 : 0;
        LocalStore.SetSound(IsOpen);
    }
    public bool IsOpenSound;

    #region H5

    [ContextMenu("Mute Sound")]
    public void MuteSound()
    {
        audioSources.ForEach(item => item.mute = true);
    }

    [ContextMenu("Unmute Sound")]
    public void UnmuteSound()
    {
        audioSources.ForEach(item => item.mute = false);
    }
    #endregion
}
