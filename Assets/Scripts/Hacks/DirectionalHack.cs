using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    [Header("Audio")]
    [SerializeField] private AudioClip writeSound;
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip failSound;

    [Header("Internal")]
    private List<HackDirection> targetSequence;
    private int currentIndex = 0;
    private bool inputAllowed = false;
    private HackMinigameController hackMinigame;
    private List<Image> spawnedArrows = new List<Image>();

    private System.Action onHackedCallback;

    void Awake()
    {
        hackMinigame = GetComponentInParent<HackMinigameController>();
    }

    public void SetupHack(List<HackDirection> sequence, bool showArrows, System.Action onHacked)
    {
        targetSequence = sequence;
        onHackedCallback = onHacked;

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

            currentArrowIcon.transform.DOPunchScale(Vector3.one * 0.3f, 0.2f, 10, 1);
            AudioManager.instance.PlaySFX(writeSound);

            currentIndex++;

            if (currentIndex >= targetSequence.Count)
            {
                WinDoor();
            }
        }
        else
        {
            spawnedArrows[currentIndex].color = wrongColor;
            spawnedArrows[currentIndex].transform.DOShakePosition(0.3f,10f);
            FailDoor();
        }
    }

    void WinDoor()
    {
        inputAllowed = false;
        Debug.Log("Secuencia correcta! Hack completado.");
        if (onHackedCallback != null)
        {
            onHackedCallback.Invoke();
        }
        AudioManager.instance.PlaySFX(successSound);
        hackMinigame.CompleteHacking();
    }

    void FailDoor()
    {
        Debug.Log("Secuencia incorrecta! Hack fallido.");
        currentIndex = 0;
        AudioManager.instance.PlaySFX(failSound);
        StartCoroutine(FlashFailEffect());
    }

    IEnumerator FlashFailEffect()
    {
        inputAllowed = false;

        Image containerIMG = arrowContainer.GetComponent<Image>();
        if (containerIMG != null)
        {
            containerIMG.color = wrongColor;
            arrowContainer.DOShakePosition(0.3f, 10f);
        }
        //arrowContainer.GetComponent<Image>().color = wrongColor;

        yield return new WaitForSeconds(0.2f);
        if (containerIMG != null)
        {
            containerIMG.color = Color.clear;
        }

        currentIndex = 0;

        for (int i = 0; i < spawnedArrows.Count; i++)
        {
            spawnedArrows[i].color = (spawnedArrows[i].sprite == hiddenSprite) ? Color.gray : normalColor;
            spawnedArrows[i].transform.localScale = Vector3.one;
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
