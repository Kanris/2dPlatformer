using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightPlatform : MonoBehaviour {

    //public float OffsetX = 20f;
    [SerializeField]
    private float MoveTime = 2f;
    [SerializeField]
    private Vector3 velocity;

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
            transform.position += (velocity * Time.fixedDeltaTime);
        }
    }

    private void ChangePlatformDirection()
    {
        velocity = velocity * -1;

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
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
