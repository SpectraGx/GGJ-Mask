using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHacker : MonoBehaviour
{
    public Node currentNode;
    public float speed = 5f;
    public MaskType currentMask = MaskType.None;

    private Node targetNode;
    private bool isMoving = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) EquipMask(MaskType.Red);
        if (Input.GetKeyDown(KeyCode.Alpha2)) EquipMask(MaskType.Blue);
        if (Input.GetKeyDown(KeyCode.Alpha3)) EquipMask(MaskType.Green);

        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            HandleMovementInput();
        }

        if (isMoving && targetNode != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetNode.transform.position, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetNode.transform.position) < 0.1f)
            {
                ArriveAtNode();
            }
        }
    }

    void HandleMovementInput()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            Node clickedNode = hit.collider.GetComponent<Node>();
            if (currentNode.neighbors.Contains(clickedNode))
            {
                    targetNode = clickedNode;
                    isMoving = true;
            }
        }
    }

    void ArriveAtNode()
    {
        currentNode = targetNode;
        targetNode = null;
        isMoving = false;
        Debug.Log("Arrived at node: " + currentNode.name);
    }

    void EquipMask(MaskType mask)
    {
        currentMask = mask;
        Debug.Log("Equipped mask: " + currentMask);
    }
}
