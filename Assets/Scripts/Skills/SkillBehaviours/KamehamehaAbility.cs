using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamehamehaAbility : SkillGameObject
{

    private void Awake()
    {
        this.animatorKey = "kamehameha";
    }


    public override void Effect()
    {
        base.Effect();
        GameObject go = Instantiate(particleSystem, cachedPlayer.transform.position, Quaternion.LookRotation(cachedPlayer.transform.forward));
        NetworkServer.Spawn(go);
        this.GetComponent<BoxCollider>().enabled = true;

    }

    [ServerCallback]
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Collided with: " + other.name);
            other.gameObject.GetComponent<HealthComponent>().TakeDamage(skill.baseDamage);
        }
    }

}
