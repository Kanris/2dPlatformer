using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats {

    public float MaxHealth = 100f;
    private float _currentHealth;
    public float CurrentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = Mathf.Clamp(value, 0, MaxHealth); }
    }
    public float speed = 300f;
    public float damage = 20f;
    public bool isAttacking = false;
}
