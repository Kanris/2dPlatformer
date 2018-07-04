using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    [SerializeField]
    private Vector3 velocity;
    [SerializeField]
    private float UpdateTime = 2f;
    private float MoveTime;
    //private bool isMoving = false;

    private void Start()
    {
        MoveTime = UpdateTime;
    }

    private void FixedUpdate()
    {
        if (Time.time >= MoveTime)
        {
            MoveTime = Time.time + UpdateTime;
            velocity *= -1;
        }

        transform.position += (velocity * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(null);
        }
    }
}
