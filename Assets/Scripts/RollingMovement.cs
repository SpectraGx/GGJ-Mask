using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RollingMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private bool isMoving = false;

    void Update()
    {
        if (isMoving) return;

        if (Input.GetKeyDown(KeyCode.W)) StartCoroutine(Move(Vector3.forward));
        else if (Input.GetKeyDown(KeyCode.S)) StartCoroutine(Move(Vector3.back));
        else if (Input.GetKeyDown(KeyCode.A)) StartCoroutine(Move(Vector3.left));
        else if (Input.GetKeyDown(KeyCode.D)) StartCoroutine(Move(Vector3.right));
    }

    IEnumerator Move (Vector3 direction)
    {
        isMoving = true;

        float remainingAngle = 90f;

        Vector3 rotationCenter = transform.position + (Vector3.down * 0.5f) + (direction * 0.5f);

        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);

        while (remainingAngle > 0f)
        {
            float angleToRotate = Mathf.Min(remainingAngle, moveSpeed * Time.deltaTime);
            transform.RotateAround(rotationCenter, rotationAxis, angleToRotate);
            remainingAngle -= angleToRotate;
            yield return null;
        }
        
        isMoving = false;
    }
}
