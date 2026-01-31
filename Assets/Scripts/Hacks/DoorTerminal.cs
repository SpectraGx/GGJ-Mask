using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTerminal : MonoBehaviour
{
    [Header("Hacks Settings")]
    public List<HackDirection> secretCode;
    public bool showCode = true;

    [Header("References")]
    public GameObject doorHackPrefab;
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
        doorHackPrefab.SetActive(true);

        DirectionalHack hackScript = doorHackPrefab.GetComponentInChildren<DirectionalHack>();

        if (hackScript != null)
        {
            hackScript.SetupHack(secretCode, showCode);
        }
        else
        {
            Debug.LogError("No se encontro el script DirectionalHack en el prefab doorHackPrefab");
        }

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
