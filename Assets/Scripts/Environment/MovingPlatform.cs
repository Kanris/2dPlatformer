using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public float m_offsetY = 20f;
    public float m_moveTime = 2f;

    Rigidbody2D body;
    bool isMoving = false;
    Vector3 nextPoint;

    // Use this for initialization
    void Start()
    {
        body = transform.GetComponent<Rigidbody2D>();

        StartCoroutine(IdleAnimation(m_offsetY));
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            Move();
        }
    }

    IEnumerator IdleAnimation(float offsetY)
    {

        nextPoint = new Vector3(body.transform.position.x, body.transform.position.y - offsetY);
        this.isMoving = true;

        m_offsetY = offsetY;
        yield return new WaitForSeconds(m_moveTime);
        this.isMoving = false;

        yield return IdleAnimation(-offsetY);
    }

    void Move()
    {

        Vector3 direction = (nextPoint - transform.position).normalized;
        //direction *= Time.fixedDeltaTime;

        //move ai
        //body.AddForce(direction * 40000 * Time.fixedDeltaTime);
        body.velocity = new Vector2(0, m_offsetY);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        //body.velocity = Vector2.zero;
        //body.angularVelocity = Vector2.zero;
    }
}
