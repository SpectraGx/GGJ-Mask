using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyTrap : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float stunDuration = 5f;
    [SerializeField] private GameObject chargeVFX;
    [SerializeField] private GameObject electricVFX;

    private bool isActive = true;
    [SerializeField]private Renderer myrenderer;

    void Start()
    {
        myrenderer = GetComponentInChildren<Renderer>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (isActive && other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy entered energy trap.");
            Discharge(other.gameObject);
        }
    }

    void Discharge(GameObject enemy)
    {
        isActive = false;
        chargeVFX.gameObject.SetActive(false);
        Debug.Log("Enemy hit by energy trap!");

        myrenderer.material.color = Color.gray;

        if (electricVFX) Instantiate(electricVFX, chargeVFX.transform.position, Quaternion.identity);

        EnemyAI enemyScript = enemy.GetComponent<EnemyAI>();
        if (enemyScript != null)
        {
            enemyScript.StunEnemy(stunDuration);
        }
    }
}
