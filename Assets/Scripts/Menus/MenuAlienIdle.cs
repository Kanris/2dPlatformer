using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAlienIdle : MonoBehaviour {

    Rigidbody2D body;
    bool isMoving = false;
    Vector3 nextPoint;

	// Use this for initialization
	void Start () {
        body = transform.GetComponent<Rigidbody2D>();

        StartCoroutine(IdleAnimation(-20));
	}

    private void FixedUpdate()
    {
        if (isMoving)
        {
            Move();
        }
    }

    IEnumerator IdleAnimation(float offsetY)
    {
        /*var logMessage = offsetY > 0 ? "Move down" : "Move Up";
        Debug.LogError(logMessage);*/

        nextPoint = new Vector3(body.transform.position.x, body.transform.position.y - offsetY);
        this.isMoving = true;

        yield return new WaitForSeconds(5f);
        this.isMoving = false;

        yield return IdleAnimation(-offsetY);
    }

    void Move()
    {
        
        Vector3 direction = (nextPoint - transform.position).normalized;
        //direction *= Time.fixedDeltaTime;

        //move ai
        body.AddForce(direction * 300 * Time.fixedDeltaTime);
    }
}
