using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmEnemy : MonoBehaviour {

    public float WaitBeforeTurnSeconds = 5f;
    private float m_waitTime;
    public float RotationOffset = -180;
    private bool m_isPlayerFound = false;

    private void Start()
    {
        m_waitTime = WaitBeforeTurnSeconds;
    }

    private void Update()
    {
        if (m_waitTime <= Time.time)
        {
            m_waitTime = Time.time + WaitBeforeTurnSeconds;
            TurnAround(-RotationOffset);
        }
    }

    private void TurnAround(float rotationOffset)
    {
        transform.Rotate(0, 0, rotationOffset);

        if (m_isPlayerFound) m_isPlayerFound = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") & !m_isPlayerFound)
        {
            m_isPlayerFound = true;
            collision.gameObject.GetComponent<Player>().playerStats.Damage(9999);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_isPlayerFound = false;
        }
    }
}
