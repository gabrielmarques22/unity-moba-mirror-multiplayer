using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMob : CharacterBase
{
    public override void Die()
    {
        base.Die();
        // Play Die Animation
        // Give XP
        // Drop loot
        // Destroy over the network. Temporary
        NetworkServer.Destroy(this.gameObject);

    }
}
