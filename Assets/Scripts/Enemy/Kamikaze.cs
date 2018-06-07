using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamikaze : MonoBehaviour {

    public EnemyStats stats;

    private void Start()
    {
        stats.Initialize(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.collider.GetComponent<Player>();

        if (player != null)
        {
            if (player.transform.position.y >= gameObject.transform.position.y)
            {
                stats.Damage(99999);
            }
            else
            {
                player.playerStats.Damage(stats.damage);
                stats.Damage(99999);
            }

        }
    }
}
