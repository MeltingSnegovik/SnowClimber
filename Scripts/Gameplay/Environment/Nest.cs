using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nest : MonoBehaviour
{
    public string spawnerName = "Nest";

    public GameObject mobTypeSpawnPref;

    public GameObject crntMob;

    public float addToSpawnPosRad;
    public Vector3 addToSpawnPosFlat;
    public Vector3 addToTransform;

    public float timeBeforeSpawn;
    public bool goingToSpawn;

    public AudioControl audioControl;

    private void Awake()
    {
        goingToSpawn = false;
        audioControl = gameObject.GetComponent<AudioControl>();
    }

    void Update()
    {
        if (crntMob == null && !goingToSpawn)
        {
            goingToSpawn = true;
            StartCoroutine(SpawnNewMob());
        }
    }
    IEnumerator SpawnNewMob()
    {
        yield return new WaitForSeconds(timeBeforeSpawn);
        audioControl.PlayAudioOneTime(spawnerName, "Spawn");
        Quaternion spawnRot = Quaternion.Euler(transform.rotation.eulerAngles + addToTransform);
        Vector3 spawnPos = transform.position + transform.right * addToSpawnPosRad + addToSpawnPosFlat;
        crntMob = Instantiate(mobTypeSpawnPref, spawnPos, spawnRot);
        goingToSpawn = false;
    }

}
