using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TraceManager : MonoBehaviour
{
    public static TraceManager Instance;

    [Header("Trace Settings")]
    [SerializeField] private float maxTrace = 100f;
    [SerializeField] private float passiveFillRate = 1.5f;
    [SerializeField] private float penaltyAmount = 15f;
    [SerializeField] private float rewardAmount = 10f;

    [Header("UI")]
    [SerializeField] private Slider traceSlider;
    [SerializeField] private Image fillImage;
    [SerializeField] private Color safeColor = Color.green;
    [SerializeField] private Color criticalColor = Color.red;

    private float currentTrace;
    private bool isTraceActive = true;

    void Awake()
    {
        if (Instance == null) Instance = this;

    }

    void Start()
    {
        currentTrace = maxTrace;
        UpdateTraceUI();
    }

    void Update()
    {
        if (!isTraceActive) return;

        if (GameManager.instance.currentState == GameState.GameOver) return;

        if (LevelUIManager.Instance.gameEnded == true)
        {
            currentTrace -= passiveFillRate * Time.deltaTime;
        }

        if (currentTrace <= 0)
        {
            currentTrace = 0;
            TriggerSystemPurge();
        }

        UpdateTraceUI();
    }

    public void ModifyTrace(float amount)
    {
        currentTrace -= amount;
        currentTrace = Mathf.Clamp(currentTrace, 0, maxTrace);
        UpdateTraceUI();
    }

    private void UpdateTraceUI()
    {
        traceSlider.value = currentTrace / maxTrace;
        if (currentTrace / maxTrace < 0.3f)
        {
            fillImage.color = Color.Lerp(criticalColor, safeColor, (currentTrace / maxTrace) / 0.3f);
        }
        else
        {
            fillImage.color = safeColor;
        }
    }

    void TriggerSystemPurge()
    {
        isTraceActive = false;
        Debug.Log("System Purge Triggered!");
        //FindObjectOfType<PlayerHealth>().TakeDamage();
        LevelUIManager.Instance.ShowLoseScreen();
    }

}
