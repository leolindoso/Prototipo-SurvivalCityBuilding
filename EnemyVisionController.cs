using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisionController : MonoBehaviour
{
    public EnemyController ThisEnemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerUnit"))
        {
            ThisEnemy.ChaseUnit(other.gameObject);
        }
    }
}
