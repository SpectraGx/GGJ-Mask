using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SequenceHack : MonoBehaviour
{

    [Header("Settings Gameplay")]
    [SerializeField] private int roundsToWin = 3;
    [SerializeField] private int baseSequenceLength = 4;
    [SerializeField] private float flashSpeed = 0.5f;


    [Header("References UI")]
    [SerializeField] private List<Image> buttons;
    [SerializeField] private List<Color> buttonColors;
    [SerializeField] private Color offColor = Color.gray;


    [Header("Internal")]
    [SerializeField] private List<int> sequence = new List<int>();
    private int currentIndex = 0;
    private int currentRound = 0;
    private bool inputAllowed = false;
    private HackMinigameController hackMinigame;

    void Start()
    {
        hackMinigame = GetComponentInParent<HackMinigameController>();
        ResetButtonVisuals();
    }

    void OnEnable()
    {
        ResetMinigame();
    }

    void OnDisable()
    {
        StopAllCoroutines();
        ResetButtonVisuals();
    }

    void ResetMinigame()
    {
        currentRound = 0;
        sequence.Clear();
        currentIndex = 0;
        inputAllowed = false;

        StopAllCoroutines();

        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        inputAllowed = false;
        SetButtonInteractable(false);

        yield return new WaitForSeconds(0.5f);

        int currentLength = baseSequenceLength + currentRound;

        for (int i = 0; i < currentLength; i++)
        {
            int randomIndex = Random.Range(0, buttons.Count);
            sequence.Add(randomIndex);

            yield return StartCoroutine(HandleInput(randomIndex));
            yield return new WaitForSeconds(0.2f);
        }

        inputAllowed = true;
        SetButtonInteractable(true);
        Debug.Log("Ingresa la secuencia ahora.");
    }

    public void OnButtonPressed(int buttonIndex)
    {
        if (!inputAllowed) return;

        StartCoroutine(HandleInput(buttonIndex));

        if (buttonIndex == sequence[currentIndex])
        {
            currentIndex++;
            if (currentIndex >= sequence.Count)
            {
                currentRound++;
                if (currentRound >= roundsToWin)
                {
                    Debug.Log("Secuencia correcta! Hack completado.");
                    inputAllowed = false;
                    hackMinigame.CompleteHacking();
                }
                else
                {
                    Debug.Log("Ronda completada. Siguiente ronda.");
                    StopAllCoroutines();
                    ResetButtonVisuals();
                    StartCoroutine(PlaySequence());
                }
            }
        }
        else
        {
            Debug.Log("Secuencia incorrecta! Intenta de nuevo.");
            TraceManager.Instance.ModifyTrace(10);
            StopAllCoroutines();
            ResetButtonVisuals();
            StartCoroutine(PlaySequence());
        }
    }

    IEnumerator HandleInput(int buttonIndex)
    {
        Color targetColor = (buttonIndex < buttonColors.Count) ? buttonColors[buttonIndex] : Color.white;
        buttons[buttonIndex].color = targetColor;

        yield return new WaitForSeconds(flashSpeed);
        buttons[buttonIndex].color = offColor;
    }

    void ResetButtonVisuals()
    {
        foreach (var btn in buttons)
        {
            btn.color = offColor;
        }
    }

    void SetButtonInteractable(bool state)
    {
        foreach (var btn in buttons)
        {
            var btnComponent = btn.GetComponent<Button>();
            if (btnComponent != null)
            {
                btnComponent.interactable = state;
            }
        }
    }
}
