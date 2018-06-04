using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public float FireRate = 0f;
    public float Distance = 100f;
    public float Damage = 10f;
    public LayerMask whatToHit;

    public Transform bulletTrailPrefab;
    public Transform muzzleFlashPrefab;

    private float timeToSpawnEffect = 0f;
    public float effectSpawnRate = 10f;

    private float timeToFire = 0f;
    private Transform firePoint;

    public float camShakeAmount = 0.1f;
    public float camShakeLength = 0.2f;
    CameraShake cameraShake;

    private void Awake()
    {
        firePoint = transform.Find("FirePoint");

        if (ReferenceEquals(firePoint, null))
        {
            Debug.LogError("No firepoint. Weapon.cs");
        }

        var gm = GameMaster.gm.gameObject;

        if (gm != null)
        {
            cameraShake = gm.GetComponent<CameraShake>();
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
        DrawBulletTrailEffect();

        cameraShake.Shake(camShakeAmount, camShakeLength);

        if (!ReferenceEquals(hit2D.collider, null))
        {
            var enemyAI = hit2D.transform.GetComponent<EnemyAI>();

            if (enemyAI != null)
            {
                enemyAI.enemyOjectStats.Damage(Damage);
            }
        }
    }

    private void DrawBulletTrailEffect()
    {
        if(Time.time >= timeToSpawnEffect)
        {
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
            Instantiate(bulletTrailPrefab, firePoint.position, firePoint.rotation);

            StartCoroutine(DrawMuzzleFlash());
        }
    }

    private IEnumerator DrawMuzzleFlash()
    {

        var muzzleFlashClone = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation);
        muzzleFlashClone.parent = firePoint;

        var size = Random.Range(0.6f, 0.9f);
        muzzleFlashClone.localScale = new Vector2(size, size);

        yield return 0; //skip 1 frame

        Destroy(muzzleFlashClone.gameObject);
    }

    private Vector2 GetMousePosition()
    {
        var mouseXPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        var mouseYPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;

        return new Vector2(mouseXPosition, mouseYPosition);
    }
}
