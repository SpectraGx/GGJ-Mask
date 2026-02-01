using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MoreMountains.Feedbacks;

public class DoorTerminal : MonoBehaviour
{
    [Header("Hacks Settings")]
    public List<HackDirection> secretCode;
    public bool showCode = true;

    [Header("References")]
    public GameObject doorHackPrefab;

    [Header("Logic & Events")]
    public UnityEvent onDoorComplete;
    public MMF_Player openFeedbacks;

    [Header("Internal")]
    private bool playerIsOnTop = false;
    private bool isHacked = false;

    void Update()
    {
        if (playerIsOnTop && !isHacked && GameManager.instance.currentState == GameState.Roaming)
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
            hackScript.SetupHack(secretCode, showCode, UnlockedDoor);
        }
        else
        {
            Debug.LogError("No se encontro el script DirectionalHack en el prefab doorHackPrefab");
        }

        Debug.Log("Hacking Started");
    }

    void UnlockedDoor()
    {
        isHacked = true;
        openFeedbacks?.PlayFeedbacks();
        onDoorComplete?.Invoke();
        Debug.Log("Door Unlocked");
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
