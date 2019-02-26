using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusic : MonoBehaviour
{
    public enum AudioPlayMode { None, Idle, Combat }

    public AudioPlayMode PlayMode { get; set; }
    public MusicTrack CurrentTrack { get; private set; }
    public bool IsChangingSongs { get; private set; } = false;
    public bool IsFadingVolume { get; private set; } = false;

    public bool Muted
    {
        get { return source.mute; }
        set { source.mute = value; }
    }

    public float CurrentVolume
    {
        get
        {
            return source.volume;
        }
        set
        {
            source.volume = Mathf.Clamp01(value);
        }
    }

    public float DefaultVolume
    {
        get
        {
            return defaultVolume;
        }
        set
        {
            defaultVolume = Mathf.Clamp01(value);
        }
    }

    [SerializeField]
    private bool shuffleOnStart;

    [Header("Tracks")]
    [SerializeField]
    private List<MusicTrack> idleTracks;
    [SerializeField]
    private List<MusicTrack> combatTracks;

    [Header("Fade Settings")]
    [SerializeField]
    private float fadeDuration = 1f;

    [Header("Pause Settings")]
    [SerializeField][Range(0f, 1f)]
    private float pauseVolumeMultiplier = 0.5f;
    //remove?
    [SerializeField]
    private float pauseVolumeFadeDuration = 1f;

    private AudioSource source;
    private float defaultVolume;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        defaultVolume = source.volume;
        PauseMenu.GamePausedEvent += OnGamePause;
        PauseMenu.GameResumedEvent += OnGameResume;
    }

    private void Start()
    {
        PlayMode = AudioPlayMode.Combat;
        if (shuffleOnStart)
        {
            ShuffleCombatTracks();
        }
    }

    private void Update()
    {
        if (Application.isFocused && !Muted)
        {
            if (Input.GetKeyDown(KeyCode.N) || !source.isPlaying || 
                source.time > CurrentTrack.FadeOutTime)
            {
                PlayNextSong();
            }
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            source.UnPause();
        }
        else
        {
            source.Pause();
        }
    }

    public void PlayNextSong()
    {
        MusicTrack nextTrack;
        switch (PlayMode)
        {
            case AudioPlayMode.None:
                CurrentTrack = null;
                return;
            case AudioPlayMode.Idle:
                nextTrack = idleTracks[0];
                idleTracks.RemoveAt(0);
                idleTracks.Add(nextTrack);
                break;
            case AudioPlayMode.Combat:
                nextTrack = combatTracks[0];
                combatTracks.RemoveAt(0);
                combatTracks.Add(nextTrack);
                break;
            default:
                CurrentTrack = null;
                return;
        }
        CurrentTrack = nextTrack;
        StartCoroutine(FadeBetweenTracks(nextTrack));

    }

    public void Pause()
    {
        source.Pause();
        enabled = false;
    }

    public void UnPause()
    {
        source.UnPause();
        enabled = true;
    }

    public void ShuffleIdleTracks()
    {
        ShuffleTracks(idleTracks);
    }

    public void ShuffleCombatTracks()
    {
        ShuffleTracks(combatTracks);
    }

    public void FadeVolume(float scale)
    {
        StartCoroutine(LerpVolume(scale, fadeDuration));
    }

    public void FadeToZero()
    {
        FadeVolume(0f);
    }

    public void FadeToDefault()
    {
        FadeVolume(defaultVolume);
    }

    private void FadeVolumePaused()
    {
        StartCoroutine(LerpVolume(defaultVolume * CurrentTrack.VolumeMultiplier *
            pauseVolumeMultiplier, pauseVolumeFadeDuration));
    }

    private void FadeVolumeResumed()
    {
        StartCoroutine(LerpVolume(defaultVolume * CurrentTrack.VolumeMultiplier,
            pauseVolumeFadeDuration));
    }

    private void OnGamePause()
    {
        if (CurrentTrack != null)
        {
            FadeVolumePaused();
        }
    }

    private void OnGameResume()
    {
        if (CurrentTrack != null)
        {
            FadeVolumeResumed();
        }
    }

    private void ShuffleTracks(List<MusicTrack> tracksToShuffle)
    {
        for (int i = 0; i < tracksToShuffle.Count; i++)
        {
            int randIndex = Random.Range(0, tracksToShuffle.Count);
            MusicTrack temp = tracksToShuffle[randIndex];
            tracksToShuffle[randIndex] = tracksToShuffle[0];
            tracksToShuffle[0] = temp;
        }
    }

    private IEnumerator LerpVolume(float scale, float duration)
    {
        float lerpStartTime = Time.unscaledTime;
        float lerpStartVolume = source.volume;
        IsFadingVolume = true;
        while (Time.unscaledTime - lerpStartTime < duration)
        {
            float lerpPercentage = (Time.unscaledTime - lerpStartTime) / duration;
            source.volume = Mathf.Lerp(lerpStartVolume, scale, lerpPercentage);
            yield return null;
        }
        source.volume = scale;
        IsFadingVolume = false;
    }

    private IEnumerator FadeBetweenTracks(MusicTrack nextTrack)
    {
        IsChangingSongs = true;
        enabled = false;
        if (source.isPlaying)
        {
            FadeToZero();
            yield return null;
            while (IsFadingVolume)
            {
                yield return null;
            }
        }
        else
        {
            source.volume = 0;
        }
        source.clip = nextTrack.Track;
        source.Play();
        source.time = nextTrack.StartTime;
        bool isPaused = PauseMenu.Instance.IsPaused;
        FadeVolume(defaultVolume * nextTrack.VolumeMultiplier * 
            (isPaused ? pauseVolumeMultiplier : 1f));
        yield return null;
        while (IsFadingVolume)
        {
            yield return null;
        }
        enabled = true;
        IsChangingSongs = false;
    }
}
