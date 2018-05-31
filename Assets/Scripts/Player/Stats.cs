using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats {

    public int Health = 100;
    public float speed = 300f;

    private GameObject gameObject;

    public Stats(GameObject thisGameObject)
    {
        this.gameObject = thisGameObject;
    }

    public void Damage(int damageFromSource)
    {
        Health -= damageFromSource;

        if (Health <= 0)
        {
            GameMaster.KillObject(gameObject);
        }
    }
}
