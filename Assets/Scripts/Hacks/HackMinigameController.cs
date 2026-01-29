using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackMinigameController : MonoBehaviour
{
    public void CompleteHacking()
    {
        gameObject.SetActive(false);
        GameManager.instance.SetGameState(GameState.Roaming);
        Debug.Log("Hacking Completed");
    }
}
