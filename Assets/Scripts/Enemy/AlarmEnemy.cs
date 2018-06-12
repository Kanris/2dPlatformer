using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmEnemy : MonoBehaviour {

    public float waitBeforeTurnSeconds = 5f;

    public bool isPlayerFound = false;

    private void Start()
    {
        StartCoroutine(TurnAround(-180));
    }

    private IEnumerator TurnAround(float rotationOffset)
    {
        transform.Rotate(0, 0, rotationOffset);

        yield return new WaitForSeconds(waitBeforeTurnSeconds);

        StartCoroutine(TurnAround(-rotationOffset));

        if (isPlayerFound) isPlayerFound = false;
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
