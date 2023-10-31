using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icicles : MonoBehaviour
{
    public string objectName;
    public float timeBeforeAmature;
   
    public bool isAmature;
//    public bool isDestroyed;
    
    public Rigidbody rigidbodyIcicle;

    public AudioControl audioControl;
    public DeathControl deathContorl;

    public GameObject goIcicle;
    public Collider goCollider;


    private void Awake()
    {
        isAmature = false;
        rigidbodyIcicle.isKinematic = true;
        deathContorl.OnDeath += Death;

        StartCoroutine(FallTimer());
    }

    IEnumerator FallTimer() {
        yield return new WaitForSeconds(timeBeforeAmature);
        rigidbodyIcicle.isKinematic = false;
        isAmature = true;
    }

    IEnumerator DeathTimer(float deathTimer)
    {
        yield return new WaitForSeconds(deathTimer);
        Destroy(gameObject);
    }

    public void Death(float deathTimer) {
        rigidbodyIcicle.isKinematic = true;
        goIcicle.SetActive(false);
        audioControl.PlayAudioClip(objectName, "Destroy");
        GameManager.Instance.AddScore(objectName);
        StartCoroutine(DeathTimer(deathTimer));
    }

    private void OnDestroy() {
        GameManager.Instance.AddScore(objectName);
    }

    public void OnTriggerEnter(Collider other) {
        if (isAmature && !deathContorl.IsDestroyed()) {
            if (other.CompareTag("Player"))
            {
                GameManager.Instance.GameOver();
            }

            if (other.CompareTag("Enemy"))
            {
                deathContorl.Death();
            }
            else {
                deathContorl.Death();
            } 
        }

    }
}
