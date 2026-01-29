using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { Roaming, Hacking, GameOver, Win, Paused }

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState currentState = GameState.Roaming;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetGameState(GameState newState)
    {
        currentState = newState;
        Debug.Log("Game State changed to: " + newState.ToString());

        if (newState == GameState.Roaming) Cursor.visible = false;
        else Cursor.visible = true;
    }
}
