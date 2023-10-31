using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCollision : MonoBehaviour
{
    public bool jumped;
    public GameObject player;

    void Awake() {
        jumped = false;
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.CompareTag("Block") || other.gameObject.CompareTag("SpeedBlock")) && jumped)
        {
            player.GetComponent<PlayerControl>()._audioControl.PlayAudioOneTime("Block", "Destroy");
            Destroy(other.gameObject);
            jumped = false;

        }
        if ((other.gameObject.CompareTag("Icicles") || other.gameObject.CompareTag("Enemy")) && jumped)
        {
            DeathControl dthCntrl = other.GetComponent<DeathControl>();
            if (dthCntrl != null)
            {
                dthCntrl.Death();
            }
            else
            {
                Destroy(other.gameObject);
            }
            jumped = false;

        }
        if (other.gameObject.CompareTag("UnbreakableBlock"))
            jumped = false;
    }
}
