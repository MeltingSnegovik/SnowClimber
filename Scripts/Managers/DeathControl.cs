using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathControl : MonoBehaviour
{
    public bool isDestroyed;
    public bool isDestroyedByPlayer;
    public float deathTimer;

    public event Action<float> OnDeath;

    public void Awake()
    {
        isDestroyed = false;
    }
    public void Death() {
        {
            isDestroyed = true;
            OnDeath(deathTimer);
        }
    }
    public bool IsDestroyed() {
        return isDestroyed;
    }
}
