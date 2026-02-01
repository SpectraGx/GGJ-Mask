using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircuitPuzzleHack : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int gridSize = 3;
    [SerializeField] private GridLayoutGroup gridLayout;
    [SerializeField] private Vector2Int startCoords= new Vector2Int(0, 0);
    [SerializeField] private Vector2Int endCoords = new Vector2Int(3,3);

    [Header("Colors")]
    [SerializeField] private Color connectedColor = Color.green;
    [SerializeField] private Color disconnectedColor = Color.blue;

    private CircuitPiece[,] grid;
    public bool IsPuzzleSolved{ get; private set; }
    private HackMinigameController hackController;

    void Awake()
    {
        hackController = GetComponentInParent<HackMinigameController>();
    }

    void OnEnable()
    {
        IsPuzzleSolved = false;
        SetupGrid();
        Invoke("CheckPuzzleState", 0.1f);
    }

    void SetupGrid()
    {
        grid = new CircuitPiece[gridSize, gridSize];
        CircuitPiece[] pieces = gridLayout.GetComponentsInChildren<CircuitPiece>();

       if (pieces.Length != gridSize * gridSize)
        {
            Debug.LogError("Las piezas no coinciden con el tamaño de la grid.");
            return;
        }

        for (int i = 0; i < pieces.Length; i++)
        {
            int x = i % gridSize;
            int y = i / gridSize;
            grid[x, y] = pieces[i];
            pieces[i].Setup(this);
            pieces[i].SetPoweredState(false, connectedColor, disconnectedColor);
        }
    }

    public void CheckPuzzleState()
    {
        foreach (var piece in grid)
        {
            piece.SetPoweredState(false, connectedColor, disconnectedColor);
        }

        bool reachedEnd = PowerFloodFill(startCoords.x, startCoords.y, Direction.None);

        if (reachedEnd)
        {
            IsPuzzleSolved = true;
            hackController.CompleteHacking();
        }
    }

    bool PowerFloodFill(int x, int y, Direction cameFrom)
    {
        if (x < 0 || x >= gridSize || y < 0 || y >= gridSize || grid[x, y].isPlaced) return false;

        CircuitPiece currentPiece = grid[x, y];

        bool canReceivePower = false;
        switch (cameFrom)
        {
            case Direction.None: canReceivePower = true; break; // Es el inicio
            case Direction.Up:    canReceivePower = currentPiece.currentConnections.up; break;
            case Direction.Down:  canReceivePower = currentPiece.currentConnections.down; break;
            case Direction.Left:  canReceivePower = currentPiece.currentConnections.left; break;
            case Direction.Right: canReceivePower = currentPiece.currentConnections.right; break;
        }

        if (!canReceivePower) return false;

        currentPiece.SetPoweredState(true, connectedColor, disconnectedColor);

        if (x == endCoords.x && y == endCoords.y) return true;

        bool pathFound = false;

        if (currentPiece.currentConnections.up) 
            if (PowerFloodFill(x, y - 1, Direction.Down)) pathFound = true;
        
        if (currentPiece.currentConnections.down)
            if (PowerFloodFill(x, y + 1, Direction.Up)) pathFound = true;
            
        if (currentPiece.currentConnections.right)
            if (PowerFloodFill(x + 1, y, Direction.Left)) pathFound = true;
            
        if (currentPiece.currentConnections.left)
            if (PowerFloodFill(x - 1, y, Direction.Right)) pathFound = true;

        return pathFound;
    }

    public enum Direction { None, Up, Down, Left, Right }
}
