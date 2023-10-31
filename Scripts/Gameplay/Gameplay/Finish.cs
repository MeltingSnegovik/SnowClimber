using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.IsGameplay())
        {
            GameManager.Instance.AddScore("Finish");
            GameManager.Instance.CompleteLevel();
            
        }
    }
}
