using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float timeToDestroy = 2f;

    void Start()
    {
        //Start the coroutine we define below named ExampleCoroutine.
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {

        //yield on a new YieldInstruction that waits for n seconds.
        yield return new WaitForSeconds(timeToDestroy);

        NetworkServer.Destroy(this.gameObject);
    }


}
