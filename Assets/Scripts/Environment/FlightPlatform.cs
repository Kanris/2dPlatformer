using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FlightPlatform : MonoBehaviour {

    public float m_offsetX = 20f;
    public float m_moveTime = 2f;

    Rigidbody2D body;
    public bool isMoving = false;
    Vector3 nextPoint;

	// Use this for initialization
	void Start () {

        body = transform.GetComponent<Rigidbody2D>();

        IdleAnimation();
	}

    private void FixedUpdate()
    {
        if (isMoving)
        {
            StartCoroutine(Move());
        }
    }

    void IdleAnimation()
    {
        this.isMoving = true;

        m_offsetX = -m_offsetX;
    }

    IEnumerator Move()
    {
        body.velocity = new Vector2(m_offsetX, 0);

        //yield return new WaitForSeconds(m_moveTime);

        this.isMoving = false;
        yield return new WaitForSeconds(5f);


        body.velocity = Vector2.zero;

        yield return new WaitForSeconds(2f);

        IdleAnimation();
    }
}
