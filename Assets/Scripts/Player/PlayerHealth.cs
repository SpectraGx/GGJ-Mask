using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxLives = 3;
    private int currentLives;
    [SerializeField] private List<Image> livesUI;

    private Vector3 respawnPosition;

    void Start()
    {
        currentLives = maxLives;
        respawnPosition = transform.position;
    }

    public void TakeDamage()
    {
        currentLives--;
        Debug.Log("Daño recibido. Vidas restantes: " + currentLives);
        UpdateLivesUI();

        ForceClosedHack();

        if (currentLives > 0)
        {
            Respawn();
        }
        else
        {
            GameOver();
        }
    }

    private void UpdateLivesUI()
    {
        for (int i=0; i<livesUI.Count; i++)
        {
            if (i < currentLives)
            {
                livesUI[i].gameObject.SetActive(true);
            }
            else
            {
                livesUI[i].gameObject.SetActive(false);
            }
        }
    }

    private void ForceClosedHack()
    {
        if (GameManager.instance != null && GameManager.instance.currentState == GameState.Hacking)
        {
            HackMinigameController activeHack = FindAnyObjectByType<HackMinigameController>();
            if (activeHack != null && activeHack.gameObject.activeSelf)
            {
                activeHack.gameObject.SetActive(false);
            }
            GameManager.instance.SetGameState(GameState.Roaming);
        }
    }

    private void Respawn()
    {
        transform.position = respawnPosition;
    }

    private void GameOver()
    {
        Debug.Log("Juego Terminado.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetCheckPoint(Vector3 newPosition)
    {
        respawnPosition = newPosition;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage();
        }
    }
}
