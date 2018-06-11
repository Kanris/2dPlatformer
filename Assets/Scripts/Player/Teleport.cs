using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {

    public Transform teleportTarget;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (teleportTarget != null)
            {
                collision.transform.position = teleportTarget.position;
            }
        }
    }

}
