using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityGate : MonoBehaviour
{
    [SerializeField] private MaskType requiredMask;
    [SerializeField] private float pushForce = 5f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CheckAccess(collision.gameObject);
        }
    }

    void CheckAccess(GameObject player)
    {
        PlayerMaskManager maskManager = player.GetComponent<PlayerMaskManager>();
        if (maskManager == null || maskManager.currentMask != requiredMask)
        {
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 pushDirection = (player.transform.position - transform.position).normalized;
                pushDirection.y = 0.2f; 

                rb.velocity = Vector3.zero;
                rb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
            }
            else
            {
                GetComponent<Collider>().isTrigger = true;
            }
        }
    }

    public bool CanPass(MaskType playerMask)
    {
        return playerMask == requiredMask;
    }
}
