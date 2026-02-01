using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HolographicBridge : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private float animationDuration = 1.5f;
    //[SerializeField] private GameObject bridgeVFX;

    void Start()
    {
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, startPoint.position);
        lineRenderer.enabled = false;
    }

    public void ActivateBridge()
    {
        lineRenderer.enabled = true;
        Vector3 finalPos = endPoint.position;

        DOTween.To(() => lineRenderer.GetPosition(1), 
                    x => lineRenderer.SetPosition(1, x), 
                    finalPos, animationDuration)
                    .SetEase(Ease.OutExpo)
                    .OnComplete(() =>
                    {
                    // Instantiate(bridgeVFX, finalPos, Quaternion.identity);
                    });
    }
}
