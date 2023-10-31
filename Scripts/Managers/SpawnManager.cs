using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Soliton<SpawnManager>
{
    public GameObject iciclesPrefab;

    public bool goingToSpawn;
    public float timeBeforeSpawnIcicles;

    public GameObject _icicles;
    public Block[] listBlocks;


    public GameObject choosenOne;

    public float addToSpawnPosRad;
    public Vector3 addToSpawnPosFlat;
    public Vector3 addToTransform;

    private void Start()
    {
        listBlocks = FindObjectsOfType<Block>();
    }

    private void Update()
    {
        if (_icicles == null && !goingToSpawn && GameManager.IsGameplay()) {
            goingToSpawn = true;
            StartCoroutine(SpawnIcicles());
        }

        if (choosenOne == null && _icicles != null)
            Destroy(_icicles);
    }
    IEnumerator SpawnIcicles() {
        yield return new WaitForSeconds(timeBeforeSpawnIcicles);
        listBlocks = FindObjectsOfType<Block>();
        List<GameObject> smallerList = new List<GameObject>();
        if (listBlocks.Length > 0)
        {
            for (int i = 0; i < listBlocks.Length; i++)
            {
                if (
                    listBlocks[i].gameObject.transform.position.y > GameManager.Instance.GetFloorHeigh() * (GameManager.Instance.GetCurrentFloor()-1)
                    && listBlocks[i].gameObject.transform.position.y <= GameManager.Instance.GetFloorHeigh() * (GameManager.Instance.GetCurrentFloor())
                    )
                {
                    smallerList.Add(listBlocks[i].gameObject);
                }
            }
            if (smallerList.Count > 0)
            {
                int crntInd = Random.Range(0, smallerList.Count);

                Vector3 spawnPos = smallerList[crntInd].transform.position + smallerList[crntInd].transform.right * addToSpawnPosRad + addToSpawnPosFlat;
                Quaternion spawnRot = Quaternion.Euler(addToTransform);
                choosenOne = smallerList[crntInd];


//                Debug.Log(spawnPos.ToString());
                _icicles = Instantiate(iciclesPrefab, spawnPos, spawnRot);
            }
            goingToSpawn = false;
        }
        else {
            goingToSpawn = false;
        }
    }
}
