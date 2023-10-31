using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : Soliton<TimeManager>
{

    public float _time;
    public bool isTicking;

    public void Start()
    {
        ResetTime();
        Pause(false);
    }

    public static void ResetTime()
    {
        Instance._time = 0f;
    }

    private void Update()
    {
        if (isTicking)
            _time += Time.deltaTime;
    }

    public static float GetTime()
    {
        return Instance._time;
    }

    public static void Pause(bool isPause)
    {
        Instance.isTicking = !isPause;
    }
}