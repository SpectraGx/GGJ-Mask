using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackTerminal : MonoBehaviour
{
    public GameObject terminalHackUIPrefab;
    private bool playerIsOnTop = false;

    void Update()
    {
        if (playerIsOnTop && GameManager.instance.currentState == GameState.Roaming)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartHacking();
            }
        }
    }

    void StartHacking()
    {
        GameManager.instance.SetGameState(GameState.Hacking);
        terminalHackUIPrefab.SetActive(true);
        Debug.Log("Hacking Started");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) playerIsOnTop = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerIsOnTop = false;
    }

}
