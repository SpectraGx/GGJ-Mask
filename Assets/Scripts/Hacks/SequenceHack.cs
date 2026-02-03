using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SequenceHack : MonoBehaviour
{
    [Header("Settings Gameplay")]
    [SerializeField] private int roundsToWin = 3;
    [SerializeField] private int baseSequenceLength = 4;
    [SerializeField] private float flashSpeed = 0.5f;
    [SerializeField] private float dmgTrace = 10f;

    [Header("References UI")]
    [SerializeField] private List<Image> buttons;
    [SerializeField] private List<Color> buttonColors;
    [SerializeField] private Color offColor = Color.gray;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip[] colorSounds; 
    [SerializeField] private AudioClip successClip;
    [SerializeField] private AudioClip failClip;

    [Header("Internal")]
    [SerializeField] private List<int> sequence = new List<int>();
    private int currentIndex = 0;
    private int currentRound = 0;
    private bool inputAllowed = false;
    private HackMinigameController hackMinigame;
    public UnityEvent onHackCompleted;

    void Start()
    {
        hackMinigame = GetComponentInParent<HackMinigameController>();

        ResetButtonVisuals();
        SetupButtonHitbox(); 
    }

    void SetupButtonHitbox()
    {
        foreach (var btn in buttons)
        {
            btn.alphaHitTestMinimumThreshold = 0.1f;
        }
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
            if (sequence.Count < currentLength)
            {
                int randomIndex = Random.Range(0, buttons.Count);
                sequence.Add(randomIndex);
            }

            yield return StartCoroutine(FlashButton(sequence[i]));
            yield return new WaitForSeconds(0.2f);
        }

        inputAllowed = true;
        SetButtonInteractable(true);
    }

    public void OnButtonPressed(int buttonIndex)
    {
        if (!inputAllowed) return;

        StartCoroutine(FlashButton(buttonIndex));

        if (buttonIndex == sequence[currentIndex])
        {
            currentIndex++;
            if (currentIndex >= sequence.Count)
            {
                currentRound++;
                if (currentRound >= roundsToWin)
                {
                    Debug.Log("Secuencia correcta! Hack completado.");
                    AudioManager.instance.PlaySFX(successClip); 
                    inputAllowed = false;
                    hackMinigame.CompleteHacking();
                    onHackCompleted?.Invoke();
                }
                else
                {
                    Debug.Log("Ronda completada. Siguiente ronda.");
                    AudioManager.instance.PlaySFX(successClip); 
                    inputAllowed = false;
                    Invoke("StartNextRound", 1f); 
                }
            }
        }
        else
        {
            Debug.Log("Secuencia incorrecta! Intenta de nuevo.");
            AudioManager.instance.PlaySFX(failClip); 

            if (TraceManager.Instance != null) TraceManager.Instance.ModifyTrace(dmgTrace);

            inputAllowed = false;

            StopAllCoroutines();
            ResetButtonVisuals();
            Invoke("RestartCurrentSequence", 1f);
        }
    }

    void StartNextRound()
    {
        sequence.Clear(); 

        currentIndex = 0;
        StartCoroutine(PlaySequence());
    }

    void RestartCurrentSequence()
    {
        currentIndex = 0;
        StartCoroutine(PlaySequenceReplay());
    }

    private IEnumerator PlaySequenceReplay()
    {
        inputAllowed = false;
        SetButtonInteractable(false);
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < sequence.Count; i++)
        {
            yield return StartCoroutine(FlashButton(sequence[i]));
            yield return new WaitForSeconds(0.2f);
        }
        inputAllowed = true;
        SetButtonInteractable(true);
    }

    IEnumerator FlashButton(int buttonIndex)
    {
        Color targetColor = (buttonIndex < buttonColors.Count) ? buttonColors[buttonIndex] : Color.white;
        buttons[buttonIndex].color = targetColor;

        if (buttonIndex < colorSounds.Length && colorSounds[buttonIndex] != null)
        {
            AudioManager.instance.PlaySFX(colorSounds[buttonIndex]);
        }

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