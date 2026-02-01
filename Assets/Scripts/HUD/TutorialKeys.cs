using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialKeys : MonoBehaviour
{
    [Header("References UI")]
    [SerializeField] private CanvasGroup w_Key;
    [SerializeField] private CanvasGroup a_Key;
    [SerializeField] private CanvasGroup s_Key;
    [SerializeField] private CanvasGroup d_Key;
    [SerializeField] private CanvasGroup e_Key;
    [SerializeField] private CanvasGroup one_Key;
    [SerializeField] private CanvasGroup two_Key;
    [SerializeField] private CanvasGroup three_Key;
    [SerializeField] private CanvasGroup r_Key;

    [Header("Animations")]
    [SerializeField] private float floatDistance = 0.25f;
    [SerializeField] private float floatDuration = 1f;

    void Start()
    {
        AnimateKey(w_Key.transform);
        AnimateKey(a_Key.transform);
        AnimateKey(s_Key.transform);
        AnimateKey(d_Key.transform);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) FadeOutKey(w_Key);
        if (Input.GetKeyDown(KeyCode.A)) FadeOutKey(a_Key);
        if (Input.GetKeyDown(KeyCode.S)) FadeOutKey(s_Key);
        if (Input.GetKeyDown(KeyCode.D)) FadeOutKey(d_Key);
        if (Input.GetKeyDown(KeyCode.E)) FadeOutKey(e_Key);
        if (Input.GetKeyDown(KeyCode.Alpha1)) FadeOutKey(one_Key);
        if (Input.GetKeyDown(KeyCode.R)) FadeOutKey(r_Key);
    }

    void AnimateKey(Transform target)
    {
        target.DOLocalMoveY(target.localPosition.y + floatDistance, floatDuration)
              .SetLoops(-1, LoopType.Yoyo)
              .SetEase(Ease.InOutSine);
    }

    void FadeOutKey(CanvasGroup key)
    {
        if (key == null || key.alpha == 0) return;
        key.DOFade(0, 0.5f).OnComplete(() => key.gameObject.SetActive(false));

        key.transform.DOScale(1.5f, 0.5f);
    }
}
