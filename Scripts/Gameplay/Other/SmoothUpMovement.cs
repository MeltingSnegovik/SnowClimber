using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothUpMovement : MonoBehaviour
{

    public float initTransformY;

    public void Start()
    {
        initTransformY = transform.position.y;
    }

    public void StartSmoothUpMovement(int crntFloor, float crntFloorHeigh , float crntSpeed) {

        StartCoroutine(SmothCameraMovement(crntFloor, crntFloorHeigh, crntSpeed));
    }

    IEnumerator SmothCameraMovement(int crntFloor, float crntFloorHeigh, float crntSpeed)
    {
        float crntHigh = transform.position.y;
        float targetHigh = initTransformY + crntFloorHeigh * (crntFloor - 1);
    
        while (!Mathf.Approximately(crntHigh, targetHigh))
        {
            if (!GameManager.IsGameplay())
                yield return null;
            float locSpeed = Mathf.MoveTowards(crntHigh, targetHigh, crntSpeed * Time.deltaTime);
            transform.Translate(Vector3.up * (locSpeed - crntHigh), Space.World);
            crntHigh = transform.position.y;

            yield return null;
        }
    }
}
