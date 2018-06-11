using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmEnemy : MonoBehaviour {

    public float waitBeforeTurnSeconds = 5f;

    private void Start()
    {
        StartCoroutine(TurnAround(-180));
    }

    private IEnumerator TurnAround(float rotationOffset)
    {
        transform.Rotate(0, 0, rotationOffset);

        yield return new WaitForSeconds(waitBeforeTurnSeconds);

        StartCoroutine(TurnAround(-rotationOffset));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().playerStats.Damage(9999);
        }
    }
}
