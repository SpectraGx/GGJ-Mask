using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TimingHack : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] RectTransform needle;
    [SerializeField] Image targetZone;
    [SerializeField] Transform targetZoneTransform;

    [Header("Settings")]
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float successAngleTolerance = 0.25f;
    [SerializeField] private int stagesToComplete = 3;
    public UnityEvent hackTimeCompleted;

    [Header("Audio")]
    [SerializeField] private AudioClip successClip;
    [SerializeField] private AudioClip failClip;


    private int currentState = 0;
    private bool isRunning = false;
    private float currentZoneAngle;

    private HackMinigameController hackController;

    void Start()
    {
        hackController = GetComponentInParent<HackMinigameController>();
    }

    void OnEnable()
    {
        ResetMinigame();
    }

    void Update()
    {
        if (!isRunning) return;
        needle.Rotate(0, 0, -rotationSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckNeedlePosition();
        }
    }

    void CheckNeedlePosition()
    {
        float needleAngle = NormalizeAngle(needle.localEulerAngles.z + 180f);
        float zoneStart = NormalizeAngle(currentZoneAngle);
        //float zoneEnd = NormalizeAngle(currentZoneAngle - (targetZone.fillAmount * 360f));
        float zoneCenter = currentZoneAngle - (targetZone.fillAmount * 360f / 2f);

        float angleDiff = Mathf.Abs(Mathf.DeltaAngle(needleAngle, zoneCenter));
        float tolerance = (targetZone.fillAmount * 360f) / 2f;

        if (angleDiff < tolerance)
        {
            Success();
        }
        else
        {
            Fail();
        }
    }

    void Success()
    {
        Debug.Log("ACIERO" + currentState);
        currentState++;
        rotationSpeed *= 1.5f;
        rotationSpeed *= -1;
        AudioManager.instance.PlaySFX(successClip);

        if (currentState >= stagesToComplete)
        {
            WinGame();
        }
        else
        {
            NextRound();
        }
    }

    void Fail()
    {
        Debug.Log("ERROR EN" + currentState);
        AudioManager.instance.PlaySFX(failClip);
        ResetMinigame();
    }

    void WinGame()
    {
        Debug.Log("Minigame Vencido!");
        hackController.CompleteHacking();
        hackTimeCompleted?.Invoke();
    }

    void NextRound()
    {
        float randomAngle = Random.Range(0f, 360f);
        targetZoneTransform.localRotation = Quaternion.Euler(0, 0, randomAngle);
        currentZoneAngle = randomAngle;
    }

    void ResetMinigame()
    {
        currentState = 0;
        rotationSpeed = 200f;
        isRunning = true;
        NextRound();
    }

    float NormalizeAngle(float angle)
    {
        angle = angle % 360f;
        if (angle < 0) angle += 360f;
        return angle;
    }

}
