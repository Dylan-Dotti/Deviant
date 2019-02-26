using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MusicTrack
{
    public AudioClip Track { get { return track; } }
    public float VolumeMultiplier { get { return volumeMultiplier; } }
    public float StartTime { get { return startTime; } }
    public float FadeOutTime { get { return fadeOutTime; } }

    [SerializeField]
    private AudioClip track;
    [SerializeField][Range(0, 1)]
    private float volumeMultiplier = 1f;
    [SerializeField]
    private float startTime = 0f;
    [SerializeField]
    private float fadeOutTime = Mathf.Infinity;

    public MusicTrack(AudioClip musicClip, float volumeMultiplier, 
        float startTime, float fadeOutTime)
    {
        track = musicClip;
        this.volumeMultiplier = Mathf.Clamp01(volumeMultiplier);
        this.startTime = startTime;
        this.fadeOutTime = fadeOutTime;
    }
}
