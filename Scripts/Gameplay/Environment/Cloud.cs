using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public Collider crntCollider;

    public GameObject cloudUpperPart;

    public float timeForTurnOn = 0.5f;

    void Start()
    {
        crntCollider = GetComponent<Collider>();

    }

    IEnumerator TrunnOnCloud() {
        yield return new WaitForSeconds(timeForTurnOn);
        cloudUpperPart.SetActive(true);
    }
        
        

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            cloudUpperPart.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(TrunnOnCloud());
        }
    }
}
