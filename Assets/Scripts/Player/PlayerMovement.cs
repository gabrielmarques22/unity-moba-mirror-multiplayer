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
    public bool canWalk = true;
    private Vector3 targetPosition;
    public LayerMask mask;
    private CameraController cameraController;

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
        // float distance = Vector3.Distance(this.transform.position, targetPosition)
        if (Input.GetMouseButtonDown(1)) {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, mask)) {
                this.Move(hit.point);
            }
        }

        if (targetPosition != null)
        {
            float distance = Vector3.Distance(this.transform.position, targetPosition);
            if(distance < 1.5)
            {
                Stop();
            }
        }
    }

    public void Move(Vector3 targetPosition)
    {
        if (!canWalk)
            return;

        cachedClickParticle.transform.position = targetPosition + Vector3.up;
        cachedClickParticle.Play();

        this.targetPosition = targetPosition;
        this.isWalking = true;
        animator.SetBool("isWalking", isWalking);
        agent.SetDestination(targetPosition);
        agent.isStopped = false;
    }

    public void Stop()
    {
        this.isWalking = false;
        animator.SetBool("isWalking", isWalking);

        agent.isStopped = true;
    }
}
