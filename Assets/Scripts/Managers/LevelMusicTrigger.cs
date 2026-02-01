using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusicTrigger : MonoBehaviour
{
    public AudioClip levelMusic;

    void Start()
    {
        if (MusicManager.Instance != null && levelMusic != null)
        {
            MusicManager.Instance.PlayMusic(levelMusic);
        }
    }
}
