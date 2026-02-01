using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private CanvasGroup mainPanel;
    [SerializeField] private CanvasGroup settingsPanel;
    [SerializeField] private CanvasGroup controlPanel;
    [SerializeField] private CanvasGroup creditsPanel;

    [Header("Scene")]
    [SerializeField] private string firstLevelName = "Level0";

    private CanvasGroup currentPanel;

    void Start()
    {
        ShowPanel(mainPanel);

        Time.timeScale = 1;
    }

    public void PlayGame()
    {
        SceneLoader.Instance.LoadLevel(firstLevelName);
    }

    public void QuitGame()
    {
        Debug.Log("Salir");
        Application.Quit();
    }

    public void OpenSettings() => SwitchPanel(settingsPanel);
    public void OpenControls() => SwitchPanel(controlPanel);
    public void OpenCredits() => SwitchPanel(creditsPanel);
    public void BackToMain() => SwitchPanel(mainPanel);

    void SwitchPanel(CanvasGroup targetPanel)
    {
        if (currentPanel == targetPanel) return;

        if (currentPanel != null)
        {
            currentPanel.blocksRaycasts = false;
            currentPanel.DOFade(0,0.3f).OnComplete(() =>
            {
               currentPanel.gameObject.SetActive(false);
            });
        }

        targetPanel.gameObject.SetActive(true);
        targetPanel.alpha = 0;
        targetPanel.DOFade(1,0.3f);
        targetPanel.blocksRaycasts = true;

        currentPanel = targetPanel;
    }

    void ShowPanel(CanvasGroup panel)
    {
        panel.gameObject.SetActive(true);
        panel.alpha = 1f;
        panel.blocksRaycasts = true;
        currentPanel = panel;

        if (settingsPanel != panel) settingsPanel.gameObject.SetActive(false);
        if (controlPanel != panel) controlPanel.gameObject.SetActive(false);
        if (creditsPanel != panel) creditsPanel.gameObject.SetActive(false);
    }

    public void OpenURL(string urlLink)
    {
        Application.OpenURL(urlLink);
    }

}
