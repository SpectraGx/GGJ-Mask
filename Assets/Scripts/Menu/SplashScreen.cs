using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] private Image logo;
    [SerializeField] private float displayDuration = 2f;
    [SerializeField] private string menuSceneName = "Menu";

    void Start()
    {
        Sequence s = DOTween.Sequence();

        logo.color = new Color(1, 1, 1, 0);
        s.Append(logo.DOFade(1, 1f));

        s.AppendInterval(displayDuration);

        s.Append(logo.DOFade(0, 1f));

        s.OnComplete(() =>
        {
            SceneLoader.Instance.LoadLevel(menuSceneName);
        });
    }
}
