using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HomingMissile : MonoBehaviour {

    private Transform m_target;
    private Rigidbody2D m_body;

    [SerializeField]
    private float RotateSpeed = 200f;

    public EnemyStats Stats;

    private void Start()
    {
        Stats.Initialize(gameObject);
        m_target = null;

        InitializeRigidbody2d();
    }

    private void InitializeRigidbody2d()
    {
        m_body = GetComponent<Rigidbody2D>();

        if (m_body == null)
            Debug.LogError("HomingMissile: Can't find Rigidbody2d on GameObject");
    }

    private float GetRotateAmount()
    {
        var direction = (Vector2)m_target.position - m_body.position;
        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        return rotateAmount;
    }

    private void FixedUpdate()
    {
        if (m_target != null)
        {
            var rotateAmount = GetRotateAmount();

            m_body.angularVelocity = -rotateAmount * RotateSpeed;
            m_body.velocity = transform.up * Stats.Speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_target = collision.transform;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().playerStats.Damage(Stats.OutputDamage);
            Stats.Damage(99999);
        }
    }
}
