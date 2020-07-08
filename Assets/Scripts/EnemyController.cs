using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : NetworkBehaviour
{

    private EnemyDetection enemyDetection;
    private Animator animator;
    private NavMeshAgent agent;
    private bool isWalking;
    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        enemyDetection = this.GetComponentInChildren<EnemyDetection>();
        if (enemyDetection == null) Debug.LogError("Enemy Detection Component not found under Enemy Game Object");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
