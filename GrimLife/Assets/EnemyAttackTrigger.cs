using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered Attack trigger");
            EnemyMovement enemyMovement = GetComponentInParent<EnemyMovement>();

            if (enemyMovement != null)
            {
                enemyMovement.SignalPrepareAttack();
            }
        }
    }
}
