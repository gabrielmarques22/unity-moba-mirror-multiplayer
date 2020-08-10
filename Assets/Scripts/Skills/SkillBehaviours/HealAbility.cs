using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAbility : SkillGameObject
{

    private void Awake()
    {
        this.animatorKey = "isHealing";
    }


    public override void Effect()
    {
        base.Effect();
        GameObject go = Instantiate(particleSystem, cachedPlayer.transform.position, Quaternion.identity);
        NetworkServer.Spawn(go);
        cachedPlayer.GetComponent<HealthComponent>().Heal(skill.baseDamage);
    }


}
