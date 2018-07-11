using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public float FireRate = 0f; //weapon firerate
    public float Distance = 100f; //weapon shooting distance
    public float Damage = 10f; //weapon damage
    public LayerMask whatToHit; //what can weapon damage

    public Transform bulletTrailPrefab; //bullet trail
    public Transform muzzleFlashPrefab; //muzzle flash

    private float timeToSpawnEffect = 0f; //when spawn next bullet trail
    public float effectSpawnRate = 10f; //max effects count

    private float timeToFire = 0f;
    private Transform firePoint; //from where spawn bullet trail and muzzle prefabs

    public float camShakeAmount = 0.1f; //camera shake amount
    public float camShakeLength = 0.2f; //camera shake length
    private CameraShake cameraShake;

    public string weaponShootSound = "DefaultShot"; //weapon sound
    private AudioManager audioManager;
    private WeaponManager weaponManager;

    //private PauseMenu pauseMenu;

    private Transform parentTransform;

    private void Start()
    {
        InitializeWeapon();
    }

    private void InitializeWeapon()
    {
        InitializeFirePoint();

        InitializeGameMaster();

        InitializeWeaponChange();

        InitalizeAudioManager();

        parentTransform = transform.parent.gameObject.transform.parent;
    }

    private void InitializeWeaponChange()
    {
        weaponManager = transform.parent.gameObject.GetComponent<WeaponManager>();
        if (weaponManager == null)
            Debug.LogError("Weapon: can't find weaponchange in parent");
    }

    private void InitalizeAudioManager()
    {
        audioManager = AudioManager.instance; //get current audiomanager
        if (audioManager == null) //if there is not audiomanager on scene show error
            Debug.LogError("Weapon: No audiomanager found in scene"); 
    }

    private void InitializeGameMaster()
    {
        var gm = GameMaster.gm.gameObject; //get current gamemaster
        if (gm != null) //if gamemaster on scene
        {
            cameraShake = gm.GetComponent<CameraShake>(); //initialize camera shake
        }
    }
	
    private void InitializeFirePoint()
    {
        firePoint = transform.GetChild(0); //initialize fire point

        if (ReferenceEquals(firePoint, null)) //if there is no firepoint on weapon show error
        {
            Debug.LogError("No firepoint. Weapon.cs");
        }
    }

	// Update is called once per frame
	void Update () {

        if (!PauseMenu.pm.IsGamePause | PauseMenu.pm == null)
        {
            ChangeEquipedWeapon();

            if (FireRate.CompareTo(0f) == 0)
            {
                if ((Input.GetButtonDown("Fire1") & !string.IsNullOrEmpty(PlayerStats.WeaponSlots[(int)WeaponType.Handgun])) 
                    | (Input.GetButtonDown("Fire2") & !string.IsNullOrEmpty(PlayerStats.WeaponSlots[(int)WeaponType.Automatic])))
                {
                    Shoot();
                }
            }
            else
            {
                if (((Input.GetButton("Fire1") & !string.IsNullOrEmpty(PlayerStats.WeaponSlots[(int)WeaponType.Handgun]) 
                      | (Input.GetButton("Fire2") & !string.IsNullOrEmpty(PlayerStats.WeaponSlots[(int)WeaponType.Automatic])))) 
                      & Time.time > timeToFire)
                {
                    timeToFire = Time.time + 1 / FireRate;
                    Shoot();
                }
            }   
        }
	}

    private void ChangeEquipedWeapon()
    {
        if (Input.GetButtonDown("Fire1") & !string.IsNullOrEmpty(PlayerStats.WeaponSlots[(int)WeaponType.Handgun]))
        {
            weaponManager.EquipWeapon(0);
        }
        else if (Input.GetButtonDown("Fire2") & !string.IsNullOrEmpty(PlayerStats.WeaponSlots[(int)WeaponType.Automatic]))
        {
            weaponManager.EquipWeapon(1);
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
            PlayWeaponSound();

            cameraShake.Shake(camShakeAmount, camShakeLength);
            
            if (!ReferenceEquals(hit2D.collider, null))
            {
                var enemyAI = hit2D.transform.GetComponent<EnemyAI>();

                if (enemyAI != null)
                {
                    enemyAI.stats.Damage(Damage + PlayerStats.AdditionalDamage);
                }
                else
                {
                    var missile = hit2D.transform.GetComponent<HomingMissile>();

                    if (missile != null)
                    {
                        missile.Stats.Damage(Damage + PlayerStats.AdditionalDamage);
                    }
                }
            }   
        }
    }

    private void PlayWeaponSound()
    {
        if (audioManager != null)
            audioManager.PlaySound(weaponShootSound);
    }

    private void DrawBulletTrailEffect()
    {
        if(Time.time >= timeToSpawnEffect)
        {
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;

            var bulletPrefab = Instantiate(bulletTrailPrefab, firePoint.position, firePoint.rotation);

            var trail = bulletPrefab.GetComponent<Transform>();
            var lineRenderer = trail.GetComponent<LineRenderer>();

            if (lineRenderer != null)
            {
                
            }

            if (parentTransform.localScale.x < 0)
            {
                var rotationOffset = firePoint.rotation.z > 0 ? 180 : -180;
                bulletPrefab.Rotate(0, 0, rotationOffset);
            }

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
