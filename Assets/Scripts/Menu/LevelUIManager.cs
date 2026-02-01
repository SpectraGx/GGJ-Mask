using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LevelUIManager : MonoBehaviour
{
    public static LevelUIManager Instance;

    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    private bool isPaused = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);
        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !winPanel.activeSelf && !losePanel.activeSelf)
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }


    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; 
        pausePanel.SetActive(true);
        
        pausePanel.transform.localScale = Vector3.zero;
        pausePanel.transform.DOScale(1f, 0.3f).SetUpdate(true); 
    }

    public void ResumeGame()
    {
        pausePanel.transform.DOScale(0f, 0.2f).SetUpdate(true).OnComplete(() => 
        {
            pausePanel.SetActive(false);
            settingsPanel.SetActive(false); 
            Time.timeScale = 1f; 
            isPaused = false;
        });
    }

    public void OpenSettings()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void BackToPause()
    {
        settingsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f; 
        SceneLoader.Instance.LoadLevel("Menu");
    }

    public void ShowWinScreen()
    {
        winPanel.SetActive(true);
    }

    public void ShowGameOver()
    {
        losePanel.SetActive(true);
    }
    
    public void RetryLevel()
    {
        Time.timeScale = 1f;
        SceneLoader.Instance.LoadLevel(SceneManager.GetActiveScene().name);
    }
}
