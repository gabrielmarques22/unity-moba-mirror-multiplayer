using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillGameObject : NetworkBehaviour
{

    public Skill skill;
    public bool isUsing = false, isCasting = false;
    private float currentTime = 0, castingTime = 0;
    [HideInInspector]
    public GameObject cachedPlayer;
    public GameObject particleSystem;
    protected string animatorKey = "";


    private void Update()
    {
        if (isUsing)
        {
            currentTime += Time.deltaTime;
            if(currentTime >= skill.skillDuration)
            {
                this.RpcFinish(cachedPlayer);
                Debug.Log("Calling End Method");
                
                isUsing = false;
                currentTime = 0;
            }
        }

        if (isCasting)
        {
            castingTime += Time.deltaTime;
            if(castingTime >= skill.castDuration)
            {
                this.Effect();
                isUsing = true;
                isCasting = false;

                castingTime = 0;
            }
        }
    }

    

    // Precasting the skill. Pointing the player who casted and plays the animation. Animation might be overritten by other classes
    [ClientRpc]
    public virtual void RpcPreSkill(GameObject obj)
    {
        Animator animator = obj.GetComponent<NetworkIdentity>().gameObject.GetComponent<Animator>();
        animator.SetBool(animatorKey, true);
        obj.GetComponent<NetworkIdentity>().gameObject.GetComponent<PlayerMovement>().canWalk = false;
        Debug.Log("Is Server: " + this.isServer);
        Debug.Log("Starting " + skill.name + " Ability on " + this.netIdentity.gameObject.name);
    }
    
    [ClientRpc]
    public virtual void RpcFinish(GameObject obj) // called after animation
    {        
        Animator animator = obj.GetComponent<NetworkIdentity>().gameObject.GetComponent<Animator>();
        animator.SetBool(animatorKey, false);
        obj.GetComponent<NetworkIdentity>().gameObject.GetComponent<PlayerMovement>().canWalk = true;

        Debug.Log("Is Server: " + this.isServer);
        Debug.Log("Finishing Heal Ability on " + this.netIdentity.gameObject.name);
    }

    // Happens on the server do effect and damage calculation
    public virtual void Effect()
    {
        Debug.Log("Server called the trigger skill");        
    }
    
    public void TriggerSkill()
    {
        RpcPreSkill(cachedPlayer);
        isCasting = true;
    }

}
