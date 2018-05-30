using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public float FireRate = 0f;
    public float Distance = 100f;
    public float Damage = 10f;
    public LayerMask whatToHit;

    private float timeToFire = 0f;
    private Transform firePoint;

    private void Awake()
    {
        firePoint = transform.Find("FirePoint");

        if (ReferenceEquals(firePoint, null))
        {
            Debug.LogError("No firepoint. Weapon.cs");
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (FireRate.CompareTo(0f) == 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButton("Fire1") & Time.time > timeToFire)
            {
                timeToFire = Time.time + 1 / FireRate;
                Shoot();
            }
        }

	}

    private void Shoot()
    {
        Vector2 mousePosition = GetMousePosition();
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);

        RaycastHit2D hit2D = Physics2D.Raycast(firePointPosition, mousePosition - firePointPosition, Distance, whatToHit);

        Debug.DrawLine(firePointPosition, (mousePosition - firePointPosition) * Distance, Color.cyan);

        if (!ReferenceEquals(hit2D.collider, null))
        {
            Debug.DrawLine(firePointPosition, hit2D.point, Color.red);
        }
    }

    private Vector2 GetMousePosition()
    {
        var mouseXPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        var mouseYPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;

        return new Vector2(mouseXPosition, mouseYPosition);
    }
}
