using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingAlarmEnemy : MonoBehaviour {

    public float waitBeforeTurnSeconds = 5f;
    private bool isPlayerFound = false;
    public float offsetX = 2f;
    private Vector2 nextPoint;


    public float waitBeforeStartMove = 2f;
    private Rigidbody2D body;
    private bool isMoving = false;

    private Vector2 velocity;

    private void Start()
    {
        StartCoroutine(TurnAround(-180));
        body = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        body.velocity = velocity;
    }

    private IEnumerator TurnAround(float rotationOffset)
    {
        transform.Rotate(0, 0, rotationOffset);

        StartMoving();

        yield return new WaitForSeconds(waitBeforeTurnSeconds);

        StopMoving();

        yield return new WaitForSeconds(waitBeforeStartMove);

        if (isPlayerFound) isPlayerFound = false;

        StartCoroutine(TurnAround(-rotationOffset));
    }

    private void StartMoving()
    {
        offsetX = -offsetX;
        velocity = new Vector2(offsetX, 0);
    }

    private void StopMoving()
    {
        velocity = new Vector2(0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") & !isPlayerFound)
        {
            isPlayerFound = true;
            collision.gameObject.GetComponent<Player>().playerStats.Damage(9999);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerFound = false;
        }
    }
}
