using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public string spawnerName = "Iglu";
    public GameObject mobTypeSpawnPref;
    public GameObject blockTypeSpawnPref;

    public GameObject crntMob;

    public float addToSpawnPosRad;
    public Vector3 addToSpawnPosFlat;
    public Vector3 addToTransform;

    public Light signalLight;
    public ParticleSystem nearLight;
    public ParticleSystem farLight;

    public float lightIntensity;

    public float timeBeforeLightOn;
    public float timeBeforeSpawn;
    public bool goingToSpawn;

    public float addToLevelHeight= -1.0f;

    public AudioControl audioControl;

    

    // Start is called before the first frame update
    void Awake()
    {
        goingToSpawn = false;
        audioControl = gameObject.GetComponent<AudioControl>();
        StopLight();
    }

    // Update is called once per frame
    void Update()
    {
        if (crntMob == null && !goingToSpawn) {
            goingToSpawn = true;
            StartCoroutine(StartLight());
            StartCoroutine(SpawnNewMob());
        }
    }

    IEnumerator SpawnNewMob() {
        yield return new WaitForSeconds(timeBeforeSpawn);
        audioControl.PlayAudioOneTime(spawnerName, "Spawn");
        Quaternion spawnRot = Quaternion.Euler(transform.rotation.eulerAngles + addToTransform);
        Vector3 spawnPos = transform.position + transform.right * addToSpawnPosRad + addToSpawnPosFlat;
        crntMob = Instantiate(mobTypeSpawnPref, spawnPos, spawnRot);
        crntMob.GetComponent<Enemy>().blockPrefab = blockTypeSpawnPref;
        crntMob.GetComponent<Enemy>().blockHeigh = transform.position.y + addToLevelHeight;
        goingToSpawn = false;
        StopLight();
    }

    IEnumerator StartLight() {
        yield return new WaitForSeconds(timeBeforeSpawn - timeBeforeLightOn);
        signalLight.intensity = lightIntensity;
        farLight.Play();
        nearLight.Play();
    }

    public void StopLight() {
        signalLight.intensity = 0;
        farLight.Stop();
        nearLight.Stop();
    }
}
