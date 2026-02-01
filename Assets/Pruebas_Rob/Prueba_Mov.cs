using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prueba_Mov : MonoBehaviour
{
    public float rotationDuration = 0.15f;
    public MMFeedbacks moveFeedback;

    private bool isRotating = false;

    void Update()
    {
        if (isRotating) return;

        if (Input.GetKeyDown(KeyCode.W))
            Rotate(Vector3.right);

        if (Input.GetKeyDown(KeyCode.S))
            Rotate(Vector3.left);

        if (Input.GetKeyDown(KeyCode.A))
            Rotate(Vector3.forward);

        if (Input.GetKeyDown(KeyCode.D))
            Rotate(Vector3.back);
    }

    void Rotate(Vector3 axis)
    {
        StartCoroutine(RotateCoroutine(axis));
        moveFeedback?.PlayFeedbacks();
    }

    System.Collections.IEnumerator RotateCoroutine(Vector3 axis)
    {
        isRotating = true;

        Quaternion startRot = transform.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(axis * 90f);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / rotationDuration;
            transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        transform.rotation = endRot;
        isRotating = false;
    }
}

