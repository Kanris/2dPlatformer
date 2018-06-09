using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour {

    public PlayerStats playerStats;
    public int yBoundaries = -20;

    public bool isInHub = false;

    private Rigidbody2D m_Rigidbody2D;

    private void Start()
    {
        playerStats.Initialize(gameObject);

        m_Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();

        if (m_Rigidbody2D == null)
        {
            Debug.LogError("Player: can't find Rigidbody2 Component");
        }

        if (gameObject.name == "HubPlayer")
        {
            isInHub = true;
        }
    }

    private void Update()
    {
        if (transform.position.y.CompareTo(yBoundaries) <= 0) //kill player after fall
        {
            if (isInHub)
            {
                transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y + 20);
                m_Rigidbody2D.velocity = new Vector3(m_Rigidbody2D.velocity.x, -20);
            }
            else
                playerStats.Damage(99999);
        }
    }

}
