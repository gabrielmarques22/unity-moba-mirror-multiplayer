using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public float lookRadius = 10f;
    private EnemyAttackComponent attackComponent; // Get Attack Component Reference so it can notify when a player is on target
    public List<GameObject> players = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> allies = new List<GameObject>();

    private void Start()
    {
        attackComponent = this.GetComponentInParent<EnemyAttackComponent>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Whenever someone enters the enemy radious it checks the collisions in Characters Layer and add based on the tag to specific lists.
        if (other.CompareTag("Enemy"))
            enemies.Add(other.gameObject);
        if (other.CompareTag("Player"))
        {
            players.Add(other.gameObject);
            attackComponent.OnEnemyInRange(other.gameObject);
        }
        if (other.CompareTag("Ally"))
            allies.Add(other.gameObject);

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
            enemies.Remove(other.gameObject);
        if (other.CompareTag("Player"))
        {
            players.Remove(other.gameObject);
            attackComponent.OnEnemyLeftRange(other.gameObject);

        }
        if (other.CompareTag("Ally"))
            allies.Remove(other.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

}
