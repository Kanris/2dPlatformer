using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour {

    public Stats playerStats;
    public int yBoundaries = -20;

    private void Start()
    {
        playerStats.Initialize(gameObject);
    }

    private void Update()
    {
        if (transform.position.y.CompareTo(yBoundaries) <= 0) //kill player after fall
        {
            playerStats.Damage(100);
        }
    }

}
