using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public float OffsetY = 20f;
    public float MoveTime = 2f;

    private Rigidbody2D body;
    private bool isMoving = false;
    private Vector3 nextPoint;

    // Use this for initialization
    void Start()
    {
        body = transform.GetComponent<Rigidbody2D>();

        StartCoroutine(ChangePlatformDirection());
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            Move();
        }
    }

    private IEnumerator ChangePlatformDirection()
    {
        nextPoint = new Vector3(body.transform.position.x, body.transform.position.y - OffsetY);
        this.isMoving = true;

        OffsetY = -OffsetY;
        yield return new WaitForSeconds(MoveTime);

        this.isMoving = false;

        yield return ChangePlatformDirection();
    }

    private void Move()
    {
        Vector3 direction = (nextPoint - transform.position).normalized;
        body.velocity = new Vector2(0, OffsetY);
    }

}
