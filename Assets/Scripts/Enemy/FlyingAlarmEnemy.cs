using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingAlarmEnemy : MonoBehaviour {

    public float waitBeforeTurnSeconds = 5f;
    private bool m_isPlayerFound = false;
    public float offsetX = 2f;
    private Vector2 m_nextPoint;


    public float waitBeforeStartMove = 2f;
    private Rigidbody2D m_rigidbody;

    private Vector2 m_velocity;

    private void Start()
    {
        StartCoroutine(TurnAround(-180));
        m_rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        m_rigidbody.velocity = m_velocity;
    }

    private IEnumerator TurnAround(float rotationOffset)
    {
        transform.Rotate(0, 0, rotationOffset);

        StartMoving();

        yield return new WaitForSeconds(waitBeforeTurnSeconds);

        StopMoving();

        yield return new WaitForSeconds(waitBeforeStartMove);

        if (m_isPlayerFound) m_isPlayerFound = false;

        StartCoroutine(TurnAround(-rotationOffset));
    }

    private void StartMoving()
    {
        offsetX = -offsetX;
        m_velocity = new Vector2(offsetX, 0);
    }

    private void StopMoving()
    {
        m_velocity = new Vector2(0, 0);
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
