using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SequenceHack : MonoBehaviour
{

    [Header("Settings Gameplay")]

    [Header("References")]
    [SerializeField] private List<Image> buttons;
    [SerializeField] private int sequenceLength = 4;
    [SerializeField] private float flashSpeed = 0.5f;

    [SerializeField] private List<int> sequence = new List<int>();
    private int currentIndex = 0;
    private bool inputAllowed = false;
    private HackMinigameController hackMinigame;

    void Start()
    {
        hackMinigame = GetComponentInParent<HackMinigameController>();
    }

    void OnEnable()
    {
        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        inputAllowed = false;
        sequence.Clear();
        currentIndex = 0;

        foreach (var btn in buttons) btn.color = Color.gray;

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < sequenceLength; i++)
        {
            int randomIndex = Random.Range(0, buttons.Count);
            sequence.Add(randomIndex);

            buttons[randomIndex].color = Color.white;
            yield return new WaitForSeconds(flashSpeed);
            buttons[randomIndex].color = Color.gray;
            yield return new WaitForSeconds(0.2f);
        }

        inputAllowed = true;
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
                Debug.Log("Secuencia correcta! Hack completado.");
                inputAllowed = false;
                hackMinigame.CompleteHacking();
            }
        }
        else
        {
            Debug.Log("Secuencia incorrecta! Intenta de nuevo.");
            TraceManager.Instance.ModifyTrace(10);
            StartCoroutine(PlaySequence());
        }
    }

    IEnumerator HandleInput(int buttonIndex)
    {
        buttons[buttonIndex].color = Color.white;
        yield return new WaitForSeconds(0.1f);
        buttons[buttonIndex].color = Color.gray;
    }
}
