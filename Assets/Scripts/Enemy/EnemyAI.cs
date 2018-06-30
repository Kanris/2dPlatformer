using System.Collections;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyAI : MonoBehaviour {
    
    //what to chase
    public Transform target;

    private float m_updateRate = 2f;
    public float updateRateSearchPlayer = 0f;

    private Seeker m_seeker;
    private Rigidbody2D m_rigidbody;

    public Path path; //calculated path
    private int m_currentWaypoint = 0;

    public float nextWaypointDistance = 3; //max distacne from the AI to a waypoint

    [HideInInspector]
    public bool pathIsEnded = false;
    [HideInInspector]
    public EnemyStats stats;

    private void Awake()
    {
        m_seeker = GetComponent<Seeker>();
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (target == null)
        {
            StartCoroutine(SearchForPlayer());
        }

        SearchForStats();

        // Path to the target position
        m_seeker.StartPath(transform.position, target.position, OnPathComplete);
        StartCoroutine( UpdatePath() );
    }

    private void SearchForStats()
    {
        var kamikaze = transform.GetComponent<Kamikaze>();

        if (kamikaze == null)
        {
            var rangeEnemy = transform.GetComponent<EnemyRangeAttack>();

            if (rangeEnemy != null)
            {
                stats = (EnemyStats)rangeEnemy.stats;
            }
        }
        else
        {
            stats = kamikaze.stats;
        }
    }


    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            m_currentWaypoint = 0;
        }
    }

    private IEnumerator UpdatePath()
    {
        if (!stats.isAttacking)
        {
            if (target == null) //if player is dead
            {
                StartCoroutine(SearchForPlayer());
                yield return new WaitForSeconds(1f / updateRateSearchPlayer);
            }
            else
            {
                // Path to the target position
                m_seeker.StartPath(transform.position, target.position, OnPathComplete);
            }   
        }

        yield return new WaitForSeconds(1f / m_updateRate);
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

            if (m_currentWaypoint >= path.vectorPath.Count)
            {
                if (pathIsEnded)
                    return;

                pathIsEnded = true;
                return;
            }

            pathIsEnded = false;

            Vector3 direction = (path.vectorPath[m_currentWaypoint] - transform.position).normalized;
            direction *= stats.Speed * Time.fixedDeltaTime;

            //move ai
            m_rigidbody.AddForce(direction);

            //move to another waypoint??
            float distance = Vector3.Distance(transform.position, path.vectorPath[m_currentWaypoint]);
            if (distance < nextWaypointDistance)
            {
                m_currentWaypoint++;
                return;
            }
        }
    }

    //enemy search for player when player is dead
    private IEnumerator SearchForPlayer()
    {
        if (target == null)
        {
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

        } else
        {
            // Path to the target position
            m_seeker.StartPath(transform.position, target.position, OnPathComplete);
            StartCoroutine(UpdatePath());
        }
    }
}
