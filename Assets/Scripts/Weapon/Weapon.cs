using System.Collections;
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

    public string weaponShootSound = "DefaultShot";
    AudioManager audioManager;
    WeaponChange weaponChange;

    private PauseMenu pauseMenu;

    private Transform parentTransform;

    private void Start()
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

        audioManager = AudioManager.instance;

        if (audioManager == null)
            Debug.LogError("Weapon: No audiomanager found in scene");

        weaponChange = transform.parent.gameObject.GetComponent<WeaponChange>();

        if (weaponChange == null)
            Debug.LogError("Weapon: can't find weaponchange in parent");

        pauseMenu = PauseMenu.pm;

        parentTransform = transform.parent.gameObject.transform.parent;
    }
	
	// Update is called once per frame
	void Update () {

        if (!pauseMenu.IsGamePause)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                weaponChange.EquipWeapon(0);
            }
            else if (Input.GetButtonDown("Fire2"))
            {
                weaponChange.EquipWeapon(1);
            }

            if (FireRate.CompareTo(0f) == 0)
            {
                if (Input.GetButtonDown("Fire1") | Input.GetButtonDown("Fire2"))
                {
                    Shoot();
                }
            }
            else
            {
                if ((Input.GetButton("Fire1") | Input.GetButton("Fire2")) & Time.time > timeToFire)
                {
                    timeToFire = Time.time + 1 / FireRate;
                    Shoot();
                }
            }   
        }
	}

    private void Shoot()
    {
        Vector2 mousePosition = GetMousePosition();
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);

        var whereToShoot = mousePosition - firePointPosition;

        if ((whereToShoot.x < 0 & parentTransform.localScale.x < 0) | (whereToShoot.x > 0 & parentTransform.localScale.x > 0))
        {
            RaycastHit2D hit2D = Physics2D.Raycast(firePointPosition, whereToShoot, Distance, whatToHit);
            DrawBulletTrailEffect();

            cameraShake.Shake(camShakeAmount, camShakeLength);

            if (audioManager != null)
                audioManager.PlaySound(weaponShootSound);
            
            if (!ReferenceEquals(hit2D.collider, null))
            {
                var enemyAI = hit2D.transform.GetComponent<EnemyAI>();

                if (enemyAI != null)
                {
                    enemyAI.stats.Damage(Damage);
                }
            }   
        }
    }

    private void DrawBulletTrailEffect()
    {
        if(Time.time >= timeToSpawnEffect)
        {
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;

            var bulletPrefab = Instantiate(bulletTrailPrefab, firePoint.position, firePoint.rotation);

            if (parentTransform.localScale.x < 0)
            {
                var rotationOffset = firePoint.rotation.z > 0 ? 180 : -180;
                bulletPrefab.Rotate(0, 0, rotationOffset);
            }
            //bulletPrefab.parent = firePoint.parent;

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
