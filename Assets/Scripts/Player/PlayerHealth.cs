using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxLives = 3;
    private int currentLives;

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
        if (currentLives > 0)
        {
            Respawn();
        }
        else
        {
            GameOver();
        }
    }

    private void Respawn()
    {
        transform.position = respawnPosition;
        Debug.Log("Jugador respawn.");
    }

    private void GameOver()
    {
        Debug.Log("Juego Terminado.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetCheckPoint(Vector3 newPosition)
    {
        respawnPosition = newPosition;
        Debug.Log("Nuevo punto de control establecido.");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage();
        }
    }
}
