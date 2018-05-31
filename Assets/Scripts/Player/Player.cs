using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour {

    public Stats playerStats;
    public int yBoundaries = -20;

    private void Awake()
    {
        playerStats = new Stats(gameObject);
    }

    private void Update()
    {
        if (transform.position.y.CompareTo(yBoundaries) <= 0) //kill player after fall
        {
            Debug.Log("Try to kill player");
            playerStats.Damage(100);
        }
    }

}
