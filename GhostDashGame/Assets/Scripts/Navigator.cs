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

    public float avoidanceRadius = 2;

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

            rb.velocity = safeVelocity(velocity);
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

    Vector2 safeVelocity(Vector2 desiredVelocity)
    {
        Vector2 testVelocity = desiredVelocity;

        GameObject[] npc = GameObject.FindGameObjectsWithTag("Enemy");

        int k = 0;
        float step = 0;
        while (! checkRVO(npc, testVelocity) && step<10)
        {
            switch (k)
            {
                case 0:
                    step += 0.1f;
                    testVelocity = desiredVelocity + new Vector2(0, step);
                    break;
                case 1:
                    testVelocity = desiredVelocity + new Vector2(step, step);
                    break;
                case 2:
                    testVelocity = desiredVelocity + new Vector2(step, 0);
                    break;
                case 3:
                    testVelocity = desiredVelocity + new Vector2(step, -step);
                    break;
                case 4:
                    testVelocity = desiredVelocity + new Vector2(0, -step);
                    break;
                case 5:
                    testVelocity = desiredVelocity + new Vector2(-step, -step);
                    break;
                case 6:
                    testVelocity = desiredVelocity + new Vector2(-step, 0);
                    break;
                case 7:
                    testVelocity = desiredVelocity + new Vector2(-step, 0);
                    break;
                case 8:
                    testVelocity = desiredVelocity / step;
                    break;
            }

            k=(k+1)%9;
        }

        Debug.Log("safeVelocity="+ testVelocity +" con step=" + step);

        return testVelocity;
    }

    bool checkRVO(GameObject[] obstacles ,Vector2 desiredVelocity)
    {
        bool safe = true;
        int k = 0;

        while (safe && k++ < obstacles.Length-1) {
            Vector2 relPos = obstacles[k].transform.position - transform.position;
            if (0.01f < relPos.magnitude && relPos.magnitude < avoidanceRadius)
            {
                Vector2 relVel = desiredVelocity - obstacles[k].GetComponent<Rigidbody2D>().velocity;

                safe = Vector2.Angle(relVel, relPos) > 80 && desiredVelocity.magnitude<=speed;
            }
        }

        return safe;
    }
}
