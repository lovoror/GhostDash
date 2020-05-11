using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Navigator : MonoBehaviour
{
    public Transform target;
    public bool navigate = true;            // enable navigation
    public float pathUpdatePeriod=0.5f;     // time interval between path updates
    public float speed = 100f;
    public float rotationSpeed = 10f;
    public float nextWaypointTreshold = 1f;

    Seeker seeker;
    Rigidbody2D rb;
    Path path;
    int waypoint;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("updatePath", 0f, pathUpdatePeriod);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(path != null && navigate)
        {
            if (waypoint < path.vectorPath.Count - 1 && Vector2.Distance(transform.position, path.vectorPath[waypoint]) < nextWaypointTreshold)
                waypoint++;

            Vector2 velocity = (path.vectorPath[waypoint] - transform.position).normalized * speed * Time.fixedDeltaTime;
            float angularvel= Vector2.SignedAngle(transform.up, path.vectorPath[waypoint] - transform.position) * rotationSpeed * Time.fixedDeltaTime;

            rb.velocity = velocity;
            rb.angularVelocity = angularvel;

            // Debug.Log("vel=" + velocity + " ang vel=" + angularvel);
        }
        else
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
        }
    }

    void updatePath()
    {
        if(target != null && seeker.IsDone()) seeker.StartPath(transform.position, target.position, onPathComplete);
    }

    void onPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            waypoint = 0;
        }
    }
}
