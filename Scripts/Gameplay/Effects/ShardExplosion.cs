using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    class for simple self-destroying objects, without any logic, after they was create (need to call startExplode())
*/
public class ShardExplosion : MonoBehaviour
{

    /*time of living*/
    public float timeShardExpl = 5.0f; 
    public void startExplode() {
        StartCoroutine(deathTimer());
    }

    IEnumerator deathTimer() {
        yield return new WaitForSeconds(timeShardExpl);
        Destroy(gameObject);
    }
}
