using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrail : MonoBehaviour {

    public int moveSpeed = 230;
    [SerializeField]
    public LayerMask LayerTohit;

	// Update is called once per frame
    void Update () {
        Debug.Log(LayerTohit.value);
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
        Destroy(gameObject, 1);
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }
}
