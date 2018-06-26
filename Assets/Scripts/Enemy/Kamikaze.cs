using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamikaze : MonoBehaviour {

    public EnemyStats stats;

    private void Start()
    {
        stats.Initialize(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<Player>();
            player.playerStats.Damage(stats.OutputDamage);
            stats.Damage(99999);
        }
    }
}
