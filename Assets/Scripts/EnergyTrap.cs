using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyTrap : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float stunDuration = 5f;
    //[SerializeField] private GameObject electricVFX;

    private bool isActive = true;
    private Renderer myrenderer;

    void Start()
    {
        myrenderer = GetComponent<Renderer>();
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
        Debug.Log("Enemy hit by energy trap!");

        myrenderer.material.color = Color.gray;

        //if (electricVFX) Instantiate(electricVFX, transform.position, Quaternion.identity);

        EnemyAI enemyScript = enemy.GetComponent<EnemyAI>();
        if (enemyScript != null)
        {
            enemyScript.StunEnemy(stunDuration);
        }
    }
}
