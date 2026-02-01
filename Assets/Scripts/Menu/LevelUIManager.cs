using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LevelUIManager : MonoBehaviour
{
    public static LevelUIManager Instance;

    [Header("Panels")]
    [SerializeField] private CanvasGroup pausePanel;
    [SerializeField] private CanvasGroup settingsPanel;
    [SerializeField] private CanvasGroup winPanel;
    [SerializeField] private CanvasGroup losePanel;

    private bool isPaused = false;
    private bool gameEnded = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        CloseAllPanels();
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (!gameEnded && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }


    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        pausePanel.gameObject.SetActive(true);

        pausePanel.alpha = 0;
        pausePanel.DOFade(1, 0.2f).SetUpdate(true);
    }

    public void ResumeGame()
    {
        pausePanel.DOFade(0, 0.2f).SetUpdate(true).OnComplete(() =>
        {
            pausePanel.gameObject.SetActive(false);
            settingsPanel.gameObject.SetActive(false); 
            Time.timeScale = 1f; 
            isPaused = false;
        });
    }


    public void OpenSettings()
    {
        pausePanel.gameObject.SetActive(false);
        settingsPanel.gameObject.SetActive(true);
        settingsPanel.alpha = 0;
        settingsPanel.DOFade(1, 0.2f).SetUpdate(true);
    }

    public void CloseSettings()
    {
        settingsPanel.DOFade(0, 0.2f).SetUpdate(true).OnComplete(() =>
        {
            settingsPanel.gameObject.SetActive(false);
            pausePanel.gameObject.SetActive(true); 
            pausePanel.alpha = 1;
        });
    }


    public void ShowWinScreen()
    {
        if (gameEnded) return;
        gameEnded = true;

        Time.timeScale = 0.5f; 

        winPanel.gameObject.SetActive(true);
        winPanel.alpha = 0;
        winPanel.DOFade(1, 1f).SetUpdate(true);
    }

    public void ShowLoseScreen()
    {
        if (gameEnded) return;
        gameEnded = true;

        losePanel.gameObject.SetActive(true);
        losePanel.alpha = 0;
        losePanel.DOFade(1, 1f).SetUpdate(true);
    }


    public void RetryLevel()
    {
        Time.timeScale = 1f; 
        if (SceneLoader.Instance != null)
            SceneLoader.Instance.LoadLevel(SceneManager.GetActiveScene().name);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        if (SceneLoader.Instance != null)
            SceneLoader.Instance.LoadLevel("Menu"); 
        else
            SceneManager.LoadScene("Menu");
    }

    public void NextLevel(string nameLevel)
    {
        Time.timeScale = 1f;
        SceneLoader.Instance.LoadLevel(nameLevel);
    }
    public void CloseAllPanels()
    {
        pausePanel.gameObject.SetActive(false);
        settingsPanel.gameObject.SetActive(false);
        winPanel.gameObject.SetActive(false);
        losePanel.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
