using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeAttack : MonoBehaviour {

    public RangeEnemyStats stats;
    private float m_updateRateSearchPlayer;
    private EnemyAI m_enemyAI;

    private Vector3 m_firePointPosition;
    public Transform bulletTrailPrefab;
    public LayerMask whatToHit;

    public Material trailMaterial;

    public string weaponShootSound;

    private AudioManager m_audioManager;

	// Use this for initialization
	void Start () {

        InitializeEnemyAI();

        InitializeAudioManager();

	}

    private void InitializeEnemyAI()
    {
        var refEnemyAI = transform.GetComponent<EnemyAI>();

        if (refEnemyAI != null)
        {
            m_enemyAI = refEnemyAI;
            m_updateRateSearchPlayer = refEnemyAI.updateRateSearchPlayer;
        }
        else
        {
            Debug.LogError("EnemyRangeAttack: Can't find EnemyStats in Parent GameObject");
        }

        stats.Initialize(gameObject);
    }

    private void InitializeAudioManager()
    {
        if (!string.IsNullOrEmpty(weaponShootSound))
            m_audioManager = AudioManager.instance;

        if (m_audioManager == null)
            Debug.LogError("EnemyRangeAttack: Can't find AudioManager.");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (m_enemyAI.target == null)
                m_enemyAI.target = collision.transform;
            
            stats.isAttacking = true;
            StartCoroutine(ShootPlayer());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            stats.isAttacking = false;
        }
    }

    private IEnumerator ShootPlayer()
    {
        if (!stats.ShotPreparing)
        {
            stats.ShotPreparing = true;

            if (m_enemyAI.target == null)
            {
                StartCoroutine( SearchForTarget() );
            } 
            else
            {
                var whereToShoot = m_enemyAI.target.position;

                m_firePointPosition = new Vector3(stats.firePoint.position.x, stats.firePoint.position.y);

                DrawShootingLine(m_firePointPosition, whereToShoot, Color.red, 0.5f);

                yield return new WaitForSeconds(0.6f);

                RaycastHit2D hit2D = Physics2D.Raycast(m_firePointPosition,
                                                       whereToShoot - m_firePointPosition,
                                                       stats.AttackRange, whatToHit);

                PlayShootSound();

                DrawBulletTrailEffect(whereToShoot);

                ShootAtTarget(hit2D);

                yield return new WaitForSeconds(stats.AttackRate);

                stats.ShotPreparing = false;

                if (stats.isAttacking)
                    StartCoroutine(ShootPlayer());
            }
        }
    }

    private void ShootAtTarget(RaycastHit2D hit2D)
    {
        if (!ReferenceEquals(hit2D.collider, null) & m_enemyAI.target == hit2D.transform)
        {
            var player = hit2D.transform.GetComponent<Player>();
            if (player != null)
            {
                player.playerStats.Damage(stats.OutputDamage);
            }
        } 
    }

    private IEnumerator SearchForTarget()
    {
        stats.ShotPreparing = false;
        yield return new WaitForSeconds(1f / m_updateRateSearchPlayer);
        StartCoroutine(ShootPlayer());
    }

    private void PlayShootSound()
    {
        if (m_audioManager != null)
            m_audioManager.PlaySound(weaponShootSound);
    }

    private void DrawShootingLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();

        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = trailMaterial;

        lr.startColor = color;
        lr.endColor = color;

        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;

        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        Destroy(myLine, duration);
    }

    private void DrawBulletTrailEffect(Vector3 whereToShoot)
    {
        Vector3 difference = whereToShoot - stats.firePoint.position;
        difference.Normalize();

        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        Instantiate(bulletTrailPrefab, stats.firePoint.position,
                    Quaternion.Euler(0f, 0f, rotationZ));
    }
}
