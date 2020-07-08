using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(CharacterBase))]
public class AttackComponent : NetworkBehaviour
{
    private CharacterBase characterBase;
    public int damage = 20;
    public float attackRange = 1.5f;
    private bool isAttacking = false;
    private bool isRanged = false;

    private GameObject currentTarget;
    public LayerMask mask;

    private Animator animator;

    public GameObject particleHolder, particle;

    private void Start()
    {
        animator = this.GetComponent<Animator>();
        // get stats from the Character Base
        characterBase = this.GetComponent<CharacterBase>();
        damage = characterBase.stats.damage.getValue();
        isRanged = characterBase.IsRanged();
        
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, mask))
            {
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    currentTarget = hit.collider.gameObject.GetComponentInParent<NetworkIdentity>().gameObject;
                    Debug.Log("Selected: " + currentTarget.name);
                }
            }
        }
        if (currentTarget != null &&
            Vector3.Distance(this.transform.position, currentTarget.transform.position) <= attackRange)
        {
            isAttacking = true;
            animator.SetBool("isAttacking", isAttacking);
            transform.LookAt(currentTarget.transform);
        } else
        {
            isAttacking = false;
            animator.SetBool("isAttacking", isAttacking);
        }
        if (isLocalPlayer)
            if (currentTarget != null)
                if (currentTarget.tag == "Enemy")
                    if (Input.GetKeyDown(KeyCode.X))
                        CmdAttack(currentTarget, netIdentity);
    }

    // Triggered by the animation on the client. Send Attack command
    public void AttackEvent()
    {
        if (currentTarget != null)
            if (currentTarget.tag == "Enemy")
            {
                CmdAttack(currentTarget, netIdentity);

            }

    }

    // Called by the client but executed on the server. Instantiate particles for everyone, also apply the damage and check if the 
    // enemy is dead to callback the RpcEnemyKilled on the Player.
    [Command]
    public void CmdAttack(GameObject enemy, NetworkIdentity client)
    {
        Transform particlePosition = client.GetComponent<AttackComponent>().particleHolder.transform;
        GameObject spawnedParticle = Instantiate(particle, particlePosition.position, particlePosition.rotation);
        NetworkServer.Spawn(spawnedParticle);

        HealthComponent hp = enemy.GetComponent<HealthComponent>();

        if (hp == null)
            return;

        // Do your own shot validation here because this runs on the server
        hp.TakeDamage(damage);
        if (hp.IsDead())
        {
            client.GetComponent<AttackComponent>().RpcEnemyKilled();
        }
    }

    // Called on the client when they kill an enemy
    [ClientRpc]
    public void RpcEnemyKilled()
    {
        Debug.Log(netId + " Player, just killed an Enemy");
        currentTarget = null;
        this.isAttacking = false;
        animator.SetBool("isAttacking", this.isAttacking);
        animator.SetBool("isWalking", false);

    }
}
