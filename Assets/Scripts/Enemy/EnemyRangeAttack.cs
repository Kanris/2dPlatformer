using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeAttack : MonoBehaviour {

    public RangeEnemyStats stats;
    private float updateRateSearchPlayer;
    private EnemyAI enemyAI;

    private Vector3 firePointPosition;
    public Transform bulletTrailPrefab;
    public LayerMask whatToHit;

	// Use this for initialization
	void Start () {

        var refEnemyAI = transform.GetComponent<EnemyAI>();

        if (refEnemyAI != null)
        {
            enemyAI = refEnemyAI;
            updateRateSearchPlayer = refEnemyAI.updateRateSearchPlayer;
        }
        else
        {
            Debug.LogError("EnemyRangeAttack: Can't find EnemyStats in Parent GameObject");
        }

        stats.Initialize(gameObject);
	}
	
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (enemyAI.target == null)
                enemyAI.target = collision.transform;
            
            stats.isAttacking = true;
            StartCoroutine(ShootPlayer());
            StartCoroutine(IdleAnimation(0.1f));
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
        //audioManager.PlaySound(weaponShootSound);

        if (!stats.shotPreparing)
        {
            stats.shotPreparing = true;

            if (enemyAI.target == null)
            {
                stats.shotPreparing = false;
                yield return new WaitForSeconds(1f / updateRateSearchPlayer);
                StartCoroutine(ShootPlayer());
            } 
            else
            {
                var whereToShoot = enemyAI.target.position;

                firePointPosition = new Vector3(stats.firePoint.position.x, stats.firePoint.position.y);
                DrawLine(firePointPosition, whereToShoot, Color.red, 0.5f);

                yield return new WaitForSeconds(0.6f);

                RaycastHit2D hit2D = Physics2D.Raycast(firePointPosition,
                                                       whereToShoot - firePointPosition,
                                                       stats.AttackRange, whatToHit);
                DrawBulletTrailEffect(whereToShoot);

                if (!ReferenceEquals(hit2D.collider, null) & enemyAI.target == hit2D.transform)
                    {
                        var player = hit2D.transform.GetComponent<Player>();
                        if (player != null)
                        {
                            player.playerStats.Damage(stats.damage);
                        }
                    }   

                yield return new WaitForSeconds(stats.AttackRate);
                stats.shotPreparing = false;

                if (stats.isAttacking)
                    StartCoroutine(ShootPlayer());
            }
        }
    }

    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        //lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));

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

    private IEnumerator IdleAnimation(float offsetY)
    {
        var rigid2d = GetComponent<Rigidbody2D>();
        rigid2d.transform.position = Vector3.MoveTowards(rigid2d.transform.position,
                                                         new Vector3(rigid2d.transform.position.x, rigid2d.transform.position.y - offsetY),
                                                         stats.speed * Time.deltaTime);

        yield return new WaitForSeconds(1f);

        if (stats.isAttacking)
            StartCoroutine(IdleAnimation(-offsetY));
    }
}
