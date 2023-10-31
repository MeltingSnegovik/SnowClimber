using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    public string bonusName;
    public float rotateSpeed = 0.5f;

    public GameObject go_Shine;
    public GameObject go_MainCamera;

    void Start() {
        go_MainCamera = GameManager.Instance.GetCurrentCamera();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            GameManager.Instance.soundAudioControl.PlayAudioOneTime("Bonus", "PickUp");
            GameManager.Instance.AddScore(bonusName);
            Destroy(gameObject);
        }
    }

    public void Update() {
        transform.Rotate(0, rotateSpeed, 0, Space.Self);
        go_Shine.transform.LookAt(go_MainCamera.transform);
    }
}
