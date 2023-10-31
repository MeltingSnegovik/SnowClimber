using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootCollision : MonoBehaviour
{
    public bool isOnGround;

    public event Action OnGround;
    public event Action OnCloud;
    public event Action OnSpeedBlock;

    public GameObject crntMovingThing;

    private void Start()
    {
        isOnGround = false;
    }

    private void OnTriggerEnter(Collider other) {
        if (false
             || other.gameObject.CompareTag("Cloud")
            )
        {
            isOnGround = true;
            crntMovingThing = other.gameObject;
            OnCloud();
        }

        if (false 
            || other.gameObject.CompareTag("SpeedBlock")
            ) 
        {
            isOnGround = true;
            crntMovingThing = other.gameObject;
            OnSpeedBlock();
        }


        if (false
            || other.gameObject.CompareTag("Ground")
            || other.gameObject.CompareTag("Block")
            || other.gameObject.CompareTag("UnbreakableBlock")
            )
        {
            isOnGround = true;
            OnGround();
        }




    }
}
