using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [Header("UI References")]
    [SerializeField] private Canvas transitionCanvas;
    [SerializeField] private Image blackScreen;
    [SerializeField] private Image whiteBar;

    [Header("Settings")]
    [SerializeField] private float duration = 0.5f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(transitionCanvas.gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        CRTTurnOn();
    }

    public void LoadLevel(string sceneName)
    {
        StartCoroutine(TransitionRoutine(sceneName));
    }

    IEnumerator TransitionRoutine(string scene)
    {
        //GameManager.instance.SetGameState(GameState.Win);

        Sequence s = DOTween.Sequence();

        blackScreen.color = Color.black;
        blackScreen.rectTransform.localScale = Vector3.one;
        blackScreen.rectTransform.DOScaleY(0.01f, 0);

        whiteBar.gameObject.SetActive(true);
        whiteBar.rectTransform.localScale = new Vector3(1, 0.01f, 1);

        s.Append(blackScreen.DOFade(1f, duration).SetEase(Ease.InOutExpo));

        yield return s.WaitForCompletion();

        AsyncOperation op = SceneManager.LoadSceneAsync(scene);
        while (!op.isDone) yield return null;
        if (GameManager.instance!= null)
        {
            GameManager.instance.SetGameState(GameState.Roaming);
        }

        CRTTurnOn();
    }

    private void CRTTurnOn()
    {
        blackScreen.color = Color.black;
        blackScreen.rectTransform.localScale = Vector3.one;

        Sequence s = DOTween.Sequence();

        s.Append(blackScreen.rectTransform.DOScaleY(0f, duration).SetEase(Ease.OutExpo));
        s.Join(blackScreen.DOFade(0f, duration / 2));
    }

}
