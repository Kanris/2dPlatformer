﻿using System.Collections;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class RangeEnemyAI : MonoBehaviour
{

    //what to chase
    public Transform target;

    public float updateRate = 2f;
    public float updateRateSearchPlayer = 0f;

    private Seeker seeker;
    private Rigidbody2D rb;

    public Path path; //calculated path
    private int currentWaypoint = 0;

    public RangeEnemyStats stats;
    public ForceMode2D fMode;

    public float nextWaypointDistance = 3; //max distacne from the AI to a waypoint

    [HideInInspector]
    public bool pathIsEnded = false;

    private Transform firePoint;
    private Vector3 firePointPosition;
    public Transform bulletTrailPrefab;
    public LayerMask whatToHit;

    private void Awake()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        firePoint = transform.Find("FirePoint");

        if (target == null)
        {
            StartCoroutine(SearchForPlayer());
        }

        stats.Initialize(gameObject);

        // Path to the target position
        seeker.StartPath(transform.position, target.position, OnPathComplete);

        StartCoroutine(UpdatePath());
    }

    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private IEnumerator UpdatePath()
    {
        if (target == null) //if player is dead
        {
            StartCoroutine(SearchForPlayer());
            yield return new WaitForSeconds(1f / updateRateSearchPlayer);
        }
        else if (!stats.isAttacking)
        {
            // Path to the target position
            seeker.StartPath(transform.position, target.position, OnPathComplete);
        }

        yield return new WaitForSeconds(1f / updateRate);
        StartCoroutine(UpdatePath());
    }

    private void FixedUpdate()
    {
        if (!stats.isAttacking)
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);

            if (target == null | path == null)
            {
                StartCoroutine(SearchForPlayer());
                return;
            }

            if (currentWaypoint >= path.vectorPath.Count)
            {
                if (pathIsEnded)
                    return;

                pathIsEnded = true;
                return;
            }

            pathIsEnded = false;

            Vector3 direction = (path.vectorPath[currentWaypoint] - transform.position).normalized;
            direction *= stats.speed * Time.fixedDeltaTime;

            //move ai
            rb.AddForce(direction, fMode);

            //move to another waypoint??
            float distance = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distance < nextWaypointDistance)
            {
                currentWaypoint++;
                return;
            }
        }
    }

    //enemy search for player when player is dead
    private IEnumerator SearchForPlayer()
    {
        if (target == null)
        {
            stats.shotPreparing = false;

            if (Time.time >= updateRateSearchPlayer)
            {
                var searchResult = GameObject.FindGameObjectWithTag("Player");

                if (searchResult != null)
                {
                    target = searchResult.transform;
                }
                else
                {

                    updateRateSearchPlayer = Time.time + 0.5f;

                    yield return new WaitForSeconds(updateRateSearchPlayer);

                    StartCoroutine(SearchForPlayer());
                }
            }

        }
        else
        {
            // Path to the target position
            seeker.StartPath(transform.position, target.position, OnPathComplete);
            StartCoroutine(UpdatePath());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
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

            if (target == null)
            {
                stats.shotPreparing = false;
                yield return new WaitForSeconds(1f / updateRateSearchPlayer);
                StartCoroutine(ShootPlayer());
            }
            else
            {
                var whereToShoot = target.position;
                firePointPosition = new Vector3(firePoint.position.x, firePoint.position.y);
                DrawLine(firePointPosition, whereToShoot, Color.red, 0.5f);

                yield return new WaitForSeconds(0.6f);

                RaycastHit2D hit2D = Physics2D.Raycast(firePointPosition, whereToShoot - firePointPosition, 
                                                       stats.AttackRange, whatToHit);
                DrawBulletTrailEffect(whereToShoot);

                if (!ReferenceEquals(hit2D.collider, null) & target == hit2D.transform)
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
        Vector3 difference = whereToShoot - firePoint.position;
        difference.Normalize();

        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        Instantiate(bulletTrailPrefab, firePoint.position,
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