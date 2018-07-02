using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FlightPlatform : MonoBehaviour {

    public float OffsetX = 20f;
    public float MoveTime = 2f;

    private Rigidbody2D body;
    private bool m_isMoving = false;
    private Vector2 target;

    private Rigidbody2D m_player;

	// Use this for initialization
	void Start () {

        body = transform.GetComponent<Rigidbody2D>();

        ChangePlatformDirection();
	}

    private void FixedUpdate()
    {
        if (m_isMoving)
        {
            var moveTowards = Vector2.MoveTowards(body.transform.position, target, Time.fixedDeltaTime * 2f);
            body.MovePosition(moveTowards);

            if (m_player != null)
            {
                var playerMoveSpeed = m_player.gameObject.GetComponent<Animator>().GetFloat("Speed");

                if (playerMoveSpeed > 0f)
                {
                    m_player = null;
                }
                else
                {
                    m_player.position = new Vector2(body.transform.position.x, m_player.transform.position.y);
                }
            }
        }
    }

    private void ChangePlatformDirection()
    {
        OffsetX = -OffsetX;
        target = new Vector2(body.transform.position.x - OffsetX, body.transform.position.y);

        this.m_isMoving = true;

        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        yield return new WaitForSeconds(MoveTime);
        this.m_isMoving = false;

        target = Vector2.zero;

        yield return new WaitForSeconds(2f);

        ChangePlatformDirection();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") & m_player == null)
        {
            m_player = collision.gameObject.GetComponent<Rigidbody2D>();
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") & m_player == null)
        {
            m_player = collision.gameObject.GetComponent<Rigidbody2D>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            m_player = null;
            collision.transform.SetParent(null);
        }
    }
}
