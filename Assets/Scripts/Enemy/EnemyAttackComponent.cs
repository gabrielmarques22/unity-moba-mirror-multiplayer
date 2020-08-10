using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackComponent : NetworkBehaviour
{
    private EnemyDetection enemyDetection;
    private CharacterBase characterBase;
    private int damage = 20;
    public float attackRange = 1.5f;
    private bool isAttacking = false;
    private bool isRanged = false;

    private GameObject currentTarget;

    private Animator animator;

    public GameObject particleHolder, particle;

    private void Start()
    {
        animator = this.GetComponent<Animator>();
        // get stats from the Character Base
        characterBase = this.GetComponent<CharacterBase>();
        damage = characterBase.stats.damage.getValue();
        isRanged = characterBase.IsRanged();
        enemyDetection = this.GetComponentInChildren<EnemyDetection>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTarget == null)
        {
            isAttacking = false;
            animator.SetBool("isAttacking", isAttacking);
        }
 

        if (currentTarget != null)
        {
            if(Vector3.Distance(this.transform.position, currentTarget.transform.position) <= attackRange)
            {

                isAttacking = true;
                animator.SetBool("isAttacking", isAttacking);
                transform.LookAt(currentTarget.transform);
            }
            else
            {
                isAttacking = false;
                animator.SetBool("isAttacking", isAttacking);
            }
        }
        

    }
    // Triggered by the animation on the client. Send Attack command
    public void AttackEvent()
    {
        if (currentTarget != null)
            if (currentTarget.tag == "Enemy" || currentTarget.CompareTag("Player"))
            {
                Debug.Log("Enemy Sending Attack");
                Attack(currentTarget, netIdentity);

            }

    }
    // Get notified when a player enters the radius area and notify when they left
    public void OnEnemyInRange(GameObject newTarget)
    {
        currentTarget = newTarget;
    }
    public void OnEnemyLeftRange(GameObject oldTarget)
    {
        // Verify if its the same target
        if (oldTarget == currentTarget)
        {
            currentTarget = null;
            // Check if there's still a player in range. If so, make him the new target
            if (enemyDetection.players.Count > 0)
            {
                Debug.Log("Player leaving the range. or dead");
                currentTarget = enemyDetection.players[0]; // @todo Improve this to aim the closest one
            } else
            {
                
                isAttacking = false;
                animator.SetBool("isAttacking", isAttacking);
            }
        }
    }
    // Called by the client but executed on the server. Instantiate particles for everyone, also apply the damage and check if the 
    // enemy is dead to callback the RpcEnemyKilled on the Player.
    public void Attack(GameObject enemy, NetworkIdentity client)
    {
        if (!isServer)
            return;
        Debug.Log(client.name + " Attack Arrived the server");
        Transform particlePosition = particleHolder.transform;
        GameObject spawnedParticle = Instantiate(particle, particlePosition.position, particlePosition.rotation);
        NetworkServer.Spawn(spawnedParticle);

        HealthComponent hp = enemy.GetComponent<HealthComponent>();
        Debug.Log("Attacking " + hp.gameObject.name);
        if (hp == null)
            return;

        // Do your own attack validation here because this runs on the server
        hp.TakeDamage(damage);
        if (hp.IsDead())
        {
            currentTarget = null;
            Debug.Log(currentTarget);
            // Enemy killed player do something
            enemy.GetComponent<CharacterBase>().Die();
            enemyDetection.RemoveDeadPlayer(enemy.gameObject);
        }
    }

}
