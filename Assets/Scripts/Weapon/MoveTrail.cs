using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrail : MonoBehaviour {

    public int moveSpeed = 230;
    [SerializeField]
    public LayerMask LayerToHit;

	// Update is called once per frame
    void Update () {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
        Destroy(gameObject, 1);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((LayerToHit & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
        {
            Destroy(gameObject);
        }
    }
}
