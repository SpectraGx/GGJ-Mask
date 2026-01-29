using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityGate : MonoBehaviour
{
    [SerializeField] private MaskType requiredMask;

    public bool CanPass(MaskType playerMask)
    {
        return playerMask == requiredMask;
    }
}
