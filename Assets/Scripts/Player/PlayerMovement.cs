using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PlayerMovement : NetworkBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    private bool isWalking;
    private Vector3 targetPosition;
    public LayerMask mask;
    private CameraController cameraController;

    [SerializeField]
    private float stoppingDistance = 1f;

    private ParticleSystem cachedClickParticle;
    [SerializeField]
    private GameObject clickParticleGO;

    // Start is called before the first frame update
    void Awake()
    {
        agent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
        cameraController = Camera.main.GetComponent<CameraController>();
        // Instantiate Particle For Caching.
        GameObject clickparticleInstantiated = Instantiate(clickParticleGO, this.transform.position, Quaternion.identity);
        cachedClickParticle = clickparticleInstantiated.GetComponent<ParticleSystem>();
    }

    public override void OnStartLocalPlayer() {
        // Send the target to the camera
        cameraController.target = this.transform;
    
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;



        if (Input.GetMouseButtonDown(1)) {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, mask)) {
                this.Move(hit.point);
            }
        }
        
        if (Vector3.Distance(this.transform.position, targetPosition) <= this.stoppingDistance && this.isWalking)
        {
            agent.isStopped = true;
            this.isWalking = false;
            animator.SetBool("isWalking", this.isWalking);
        }
    }

    public void Move(Vector3 targetPosition)
    {
        cachedClickParticle.transform.position = targetPosition + Vector3.up;
        cachedClickParticle.Play();

        this.targetPosition = targetPosition;
        this.isWalking = true;
        animator.SetBool("isWalking", isWalking);
        agent.SetDestination(targetPosition);
        agent.isStopped = false;

    }
}
