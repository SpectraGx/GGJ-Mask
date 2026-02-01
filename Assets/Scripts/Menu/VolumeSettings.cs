using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string parameterName = "MasterVol";

    [Header("UI References")]
    [SerializeField] private Slider slider;

    void Start()
    {
        float savedValue = PlayerPrefs.GetFloat(parameterName,0.75f);
        slider.value = savedValue;

        SetVolume(savedValue);

        slider.onValueChanged.AddListener(val => SetVolume(val));
    }

    public void SetVolume(float value)
    {
        Debug.Log($"[VolumeSettings]{parameterName} Input:{value}");
        float dbVolume = Mathf.Log10(Mathf.Clamp(value,0.0001f,1f))*20;

        Debug.Log($"[Volume Settings]{parameterName} set to {dbVolume} db");

        audioMixer.SetFloat(parameterName,dbVolume);

        PlayerPrefs.SetFloat(parameterName, value);
    }
}
