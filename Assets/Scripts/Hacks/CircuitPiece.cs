using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CircuitPiece : MonoBehaviour, IPointerClickHandler
{
    [System.Serializable]
    public struct Connections
    {
        public bool up, down, left, right;
    }

    [Header("Connections Settings")]
    [SerializeField] private Connections baseConnections;

    [Header("Current State")]
    public Connections currentConnections;
    public bool isPlaced = false;

    private Image myImage;
    private CircuitPuzzleHack controller;
    private int currentRotaionIndex = 0;

    void Awake()
    {
        if (myImage == null) myImage = GetComponent<Image>();
        currentConnections = baseConnections;
    }

    public void Setup(CircuitPuzzleHack mainController)
    {
        if (myImage == null) myImage = GetComponent<Image>();
        controller = mainController;

        int randomRotations = Random.Range(0, 4);
        for (int i = 0; i < randomRotations; i++)
        {
            RotatePiece();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (controller.IsPuzzleSolved) return;

        RotatePiece();
        controller.CheckPuzzleState();
    }

    private void RotatePiece()
    {
        transform.Rotate(0f, 0f, -90f);
        currentRotaionIndex = (currentRotaionIndex + 1) % 4;

        bool oldUp = currentConnections.up;
        currentConnections.up = currentConnections.left;
        currentConnections.left = currentConnections.down;
        currentConnections.down = currentConnections.right;
        currentConnections.right = oldUp;
    }

    public void SetPoweredState(bool powered, Color poweredColor, Color unpoweredColor)
    {
        if (myImage == null) myImage = GetComponent<Image>();
        isPlaced = powered;
        myImage.color = isPlaced ? poweredColor : unpoweredColor;
    }
}
