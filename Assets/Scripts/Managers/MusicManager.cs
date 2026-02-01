using UnityEngine;
using DG.Tweening; 
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Settings")]
    public AudioMixerGroup musicGroup; 
    public float crossfadeDuration = 2f;

    private AudioSource sourceA;
    private AudioSource sourceB;
    private bool isPlayingSourceA = true; 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetupAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void SetupAudioSources()
    {
        sourceA = gameObject.AddComponent<AudioSource>();
        sourceB = gameObject.AddComponent<AudioSource>();

        sourceA.outputAudioMixerGroup = musicGroup;
        sourceB.outputAudioMixerGroup = musicGroup;
        sourceA.loop = true;
        sourceB.loop = true;
        sourceA.playOnAwake = false;
        sourceB.playOnAwake = false;
    }

    public void PlayMusic(AudioClip newClip)
    {
        AudioSource activeSource = isPlayingSourceA ? sourceA : sourceB;
        AudioSource newSource = isPlayingSourceA ? sourceB : sourceA;

        if (activeSource.clip == newClip && activeSource.isPlaying) return;

        if (activeSource.clip == null || !activeSource.isPlaying)
        {
            newSource.clip = newClip;
            newSource.volume = 0;
            newSource.Play();
            newSource.DOFade(1f, crossfadeDuration);
            isPlayingSourceA = !isPlayingSourceA;
            return;
        }


        activeSource.DOFade(0f, crossfadeDuration).OnComplete(() => activeSource.Stop());

        newSource.clip = newClip;
        newSource.volume = 0f;
        newSource.Play();
        newSource.DOFade(1f, crossfadeDuration);

        isPlayingSourceA = !isPlayingSourceA;
    }
}