using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum HackDirection { Up, Down, Left,Right}

public class DirectionalHack : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform arrowContainer;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private Sprite hiddenSprite;

    [Header("Colors")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color correctColor = Color.green;
    [SerializeField] private Color wrongColor = Color.red;

    [Header("Internal")]
    private List<HackDirection> targetSequence;
    private int currentIndex = 0;
    private bool inputAllowed = false;
    private HackMinigameController hackMinigame;
    private List<Image> spawnedArrows = new List<Image>();

    void Awake()
    {
        hackMinigame = GetComponentInParent<HackMinigameController>();
    }

    public void SetupHack(List<HackDirection> sequence, bool showArrows)
    {
        targetSequence = sequence;
        currentIndex = 0;
        inputAllowed = true;

        foreach (Transform child in arrowContainer) Destroy(child.gameObject);
        spawnedArrows.Clear();

        foreach (HackDirection dir in targetSequence)
        {
            GameObject arrowObj = Instantiate(arrowPrefab, arrowContainer);
            Image arrowImage = arrowObj.GetComponent<Image>();

            if (showArrows)
            {
                arrowImage.sprite = arrowSprite;
                arrowImage.transform.rotation = GetRotationForDirection(dir);
                arrowImage.color = normalColor;
            }
            else
            {
                arrowImage.sprite = hiddenSprite;
                arrowImage.transform.rotation = Quaternion.identity;
                arrowImage.color = normalColor;
            }
            spawnedArrows.Add(arrowImage);
        }
    }

    void Update()
    {
        if (!inputAllowed) return;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) ProcessInput(HackDirection.Up);
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) ProcessInput(HackDirection.Down);
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) ProcessInput(HackDirection.Left);
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) ProcessInput(HackDirection.Right);

    }

    void ProcessInput(HackDirection input)
    {
        if (input == targetSequence[currentIndex])
        {
            Image currentArrowIcon = spawnedArrows[currentIndex];

            currentArrowIcon.sprite = arrowSprite;
            currentArrowIcon.transform.rotation = GetRotationForDirection(input);
            currentArrowIcon.color = correctColor;

            currentIndex++;

            if (currentIndex >= targetSequence.Count)
            {
                WinDoor();
            }
        }
        else
        {
            spawnedArrows[currentIndex].color = wrongColor;
            FailDoor();
        }
    }

    void WinDoor()
    {
        inputAllowed = false;
        Debug.Log("Secuencia correcta! Hack completado.");
        hackMinigame.CompleteHacking();
    }

    void FailDoor()
    {
        Debug.Log("Secuencia incorrecta! Hack fallido.");
        currentIndex = 0;
        StartCoroutine(FlashFailEffect());
    }

    IEnumerator FlashFailEffect()
    {
        inputAllowed = false;
        arrowContainer.GetComponent<Image>().color = wrongColor;
        yield return new WaitForSeconds(0.2f);
        arrowContainer.GetComponent<Image>().color = Color.clear;

        currentIndex = 0;

        for (int i = 0; i < spawnedArrows.Count; i++)
        {
            spawnedArrows[i].color = (spawnedArrows[i].sprite == hiddenSprite) ? Color.gray : normalColor;
        }

        inputAllowed = true;
    }

    Quaternion GetRotationForDirection(HackDirection dir)
    {
        switch (dir)
        {
            case HackDirection.Up:
                return Quaternion.Euler(0, 0, 0);
            case HackDirection.Down:
                return Quaternion.Euler(0, 0, 180);
            case HackDirection.Left:
                return Quaternion.Euler(0, 0, 90);
            case HackDirection.Right:
                return Quaternion.Euler(0, 0, -90);
            default:
                return Quaternion.identity;
        }
    }
}
