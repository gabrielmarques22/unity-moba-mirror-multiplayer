using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public float lookRadius = 10f;
    private EnemyAttackComponent attackComponent; // Get Attack Component Reference so it can notify when a player is on target
    private EnemyController controllerComponent; // Get reference to notify @todo refactor for event based notification
    public List<GameObject> players = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> allies = new List<GameObject>();

    private void Start()
    {
        controllerComponent = this.GetComponentInParent<EnemyController>();
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
            controllerComponent.OnEnemyInRange(other.gameObject);
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
            controllerComponent.OnEnemyLeftRange(other.gameObject);

        }
        if (other.CompareTag("Ally"))
            allies.Remove(other.gameObject);
    }

    public void RemoveDeadPlayer(GameObject player)
    {
        if (players.Contains(player))
        {
            attackComponent.OnEnemyLeftRange(player);
            players.Remove(player);

        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

}
