using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : NetworkBehaviour
{

    private EnemyDetection enemyDetection;
    // @todo move current target to enemy detection, so we can just look there. Or make an event to raise here whenever someone enters the area
    private GameObject currentTarget;
    private Animator animator;
    private NavMeshAgent agent;
    private bool isWalking;
    private Vector3 targetPosition;

    public float followRange = 10f;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        agent = this.GetComponent<NavMeshAgent>();
        enemyDetection = this.GetComponentInChildren<EnemyDetection>();
        if (enemyDetection == null) Debug.LogError("Enemy Detection Component not found under Enemy Game Object");
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTarget != null)
        {
            float distance = Vector3.Distance(this.transform.position, currentTarget.transform.position);
            if (distance <= followRange)
            {

                agent.SetDestination(currentTarget.transform.position);
                if(distance <= agent.stoppingDistance)
                {
                    agent.isStopped = true;
                    this.isWalking = false;
                    animator.SetBool("isWalking", this.isWalking);
                } else { 
                    agent.isStopped = false;
                    this.isWalking = true;
                    animator.SetBool("isWalking", this.isWalking);
                }
            }
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
                currentTarget = enemyDetection.players[0]; // @todo Improve this to aim the closest one
            }
        }
    }



}
