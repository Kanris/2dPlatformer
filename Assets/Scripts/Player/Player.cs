using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour {

    public Stats playerStats;
    public ObjectStats playerObjectStats;
    public int yBoundaries = -20;

    private void Awake()
    {
        playerObjectStats = new ObjectStats(gameObject, playerStats);
    }

    private void Update()
    {
        if (transform.position.y.CompareTo(yBoundaries) <= 0) //kill player after fall
        {
            playerObjectStats.Damage(100);
        }
    }

}
