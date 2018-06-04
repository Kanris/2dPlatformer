using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyAI : MonoBehaviour {

    //what to chase
    public Transform target;

    public float updateRate = 2f;
    public float updateRateSearchPlayer = 0f;

    private Seeker seeker;
    private Rigidbody2D rb;

    public Path path; //calculated path
    private int currentWaypoint = 0;

    public Stats enemyStats;
    public ForceMode2D fMode;

    public float nextWaypointDistance = 3; //max distacne from the AI to a waypoint

    [HideInInspector]
    public bool pathIsEnded = false;

    private void Awake()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        enemyStats = new Stats(gameObject);

        if (target == null)
        {
            Debug.LogError("No player to chase.");
            return;
        }

        // Path to the target position
        seeker.StartPath(transform.position, target.position, OnPathComplete);

        StartCoroutine( UpdatePath() );
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
            SearchForPlayer(); //search for player
            yield return new WaitForSeconds(1f / updateRateSearchPlayer);
        }
        else
        {
            // Path to the target position
            seeker.StartPath(transform.position, target.position, OnPathComplete);
            yield return new WaitForSeconds(1f / updateRate);
        }

        StartCoroutine(UpdatePath());
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);

        if (target == null | path == null)
        {
            SearchForPlayer();
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            if (pathIsEnded)
                return;

            Debug.Log("End of path reached");
            pathIsEnded = true;
            return;
        }

        pathIsEnded = false;

        Vector3 direction = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        direction *= enemyStats.speed * Time.fixedDeltaTime;

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

    //enemy search for player when player is dead
    private void SearchForPlayer()
    {
        if (Time.time >= updateRateSearchPlayer)
        {
            var searchResult = GameObject.FindGameObjectWithTag("Player");

            if (searchResult != null)
            {
                target = searchResult.transform;
            }

            updateRateSearchPlayer = Time.time + 0.5f;
        }
    }
}
