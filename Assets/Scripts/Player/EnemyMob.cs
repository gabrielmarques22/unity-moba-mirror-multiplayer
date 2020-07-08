using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMob : CharacterBase
{
    public override void Die()
    {
        base.Die();
        // Play Die Animation
        // Destroy over the network
    }
}
