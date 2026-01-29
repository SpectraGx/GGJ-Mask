using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RollingMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] LayerMask obstacleLayer;
    private bool isMoving = false;
    private PlayerMaskManager maskManager;

    void Start()
    {
        maskManager = GetComponent<PlayerMaskManager>();
    }

    void Update()
    {
        if (isMoving) return;

        if (Input.GetKeyDown(KeyCode.W)) TryMove(Vector3.forward);
        else if (Input.GetKeyDown(KeyCode.S)) TryMove(Vector3.back);
        else if (Input.GetKeyDown(KeyCode.A)) TryMove(Vector3.left);
        else if (Input.GetKeyDown(KeyCode.D)) TryMove(Vector3.right);
    }

    void TryMove(Vector3 direction)
    {
        if (!IsBlocked(direction))
        {
            StartCoroutine(Move(direction));
        }
    }

    bool IsBlocked(Vector3 direction)
    {

        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, 1f))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                Debug.Log("Blocked by obstacle");
                return true;
            }

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Gate"))
            {
                SecurityGate gate = hit.collider.GetComponent<SecurityGate>();
                if (gate != null)
                {
                    if (gate.CanPass(maskManager.currentMask))
                    {
                        Debug.Log("Acceso Concecido");
                        return false;
                    }
                    else
                    {
                        Debug.Log("Acceso Denegado");
                        return true;
                    }
                }
            }
        }
        Debug.DrawRay(transform.position, direction * 1f, Color.green, 1f);
        return false;
    }

    IEnumerator Move(Vector3 direction)
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

        RoundPosition();

        isMoving = false;
    }

    void RoundPosition()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Round(pos.x);
        pos.y = 0.5f;
        pos.z = Mathf.Round(pos.z);
        transform.position = pos;
    }
}
