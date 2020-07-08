using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject enemyToSpawn;

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject orc = Instantiate(enemyToSpawn);
            NetworkServer.Spawn(orc);
        }
    }
}
