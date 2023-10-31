using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public GameObject shardExplosionPrefab;


    private void OnDestroy()
    {
        if (GameManager.IsGameplay())
        {
            GameObject crntShardExpl = Instantiate(shardExplosionPrefab, transform.position, transform.rotation);
            crntShardExpl.GetComponent<ShardExplosion>().startExplode();
            GameManager.Instance.AddScore("Block");
        }
    }

}
