using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FlightPlatform : MonoBehaviour {

    public float m_offsetX = 20f;
    public float m_moveTime = 2f;

    Rigidbody2D body;
    public bool isMoving = false;
    Vector2 target;

    public Rigidbody2D player;
    public bool isPlayerMoving = false;

	// Use this for initialization
	void Start () {

        body = transform.GetComponent<Rigidbody2D>();

        ChangePlatformDirection();
	}

    private void FixedUpdate()
    {
        if (isMoving)
        {
            var moveTowards = Vector2.MoveTowards(body.transform.position, target, Time.fixedDeltaTime * 2f);
            body.MovePosition(moveTowards);

            if (player != null)
            {
                var playerMoveSpeed = player.gameObject.GetComponent<Animator>().GetFloat("Speed");

                if (playerMoveSpeed > 0f)
                {
                    player = null;
                    isPlayerMoving = true;
                }
                else
                {
                    isPlayerMoving = false;
                }

                if (player != null)
                {
                    player.position = new Vector2(body.transform.position.x, player.transform.position.y);
                }
            }
        }
    }

    private void ChangePlatformDirection()
    {
        m_offsetX = -m_offsetX;
        target = new Vector2(body.transform.position.x - m_offsetX, body.transform.position.y);

        this.isMoving = true;

        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        yield return new WaitForSeconds(m_moveTime);
        this.isMoving = false;

        target = Vector2.zero;

        yield return new WaitForSeconds(2f);

        ChangePlatformDirection();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" & player == null)
        {
            player = collision.gameObject.GetComponent<Rigidbody2D>();
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" & player == null)
        {
            player = collision.gameObject.GetComponent<Rigidbody2D>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player = null;
            collision.transform.SetParent(null);
        }
    }
}
