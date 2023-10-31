using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpaceChecker : MonoBehaviour
{

    public bool spaceEmpty;
    
    void Awake()
    {
        spaceEmpty = true;
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Block") || other.CompareTag("UnbreakableBlock"))
        {
            spaceEmpty = false;
            return;
        }
        spaceEmpty = true;
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Block") || other.CompareTag("UnbreakableBlock"))
        {
            spaceEmpty = true;
        }
    }
}
