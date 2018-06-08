using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAlienIdle : MonoBehaviour {

    Rigidbody2D body;
    bool isMoving = false;

	// Use this for initialization
	void Start () {
        body = transform.GetComponent<Rigidbody2D>();

        StartCoroutine(IdleAnimation(-5));
	}

    IEnumerator IdleAnimation(float offsetY)
    {
        for (int index = 0; index < 4; index++)
        {
            body.transform.position = Vector3.MoveTowards(body.transform.position,
                                                 new Vector3(body.transform.position.x, body.transform.position.y - offsetY),
                                                    100f * Time.deltaTime);

            yield return new WaitForSeconds(0.5f);   
        }

        yield return IdleAnimation(-offsetY);
    }
}
