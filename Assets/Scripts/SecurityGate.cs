using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityGate : MonoBehaviour
{
    [SerializeField] private MaskType requiredMask;
    [SerializeField] private AudioClip lockPassAudioClip;

    public bool TryEnter(PlayerMaskManager maskManager)
    {
        if (maskManager.currentMask == requiredMask)
        {
            GetComponent<Collider>().isTrigger = true;
            return true;
        }
        else
        {
            Debug.Log("Gate locked. Required mask: " + requiredMask);
            AudioManager.instance.PlaySFX(lockPassAudioClip);
            return false;
        }
    }
}
