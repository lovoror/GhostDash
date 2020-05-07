using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{

    GameObject player;
    PandaBehaviour tree;
    AIDestinationSetter destinationsetter;
    AIPath pathPlanner;

    GameObject[] patrolPath;
    int waypoint = 0;

    float viewAngle = 70;
    float viewDistance = 100;
    LayerMask mask;

    GameObject lastKnownPlayerPos;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        tree = GetComponent<PandaBehaviour>();
        destinationsetter = GetComponent<AIDestinationSetter>();
        pathPlanner = GetComponent<AIPath>();

        mask = LayerMask.GetMask("Player", "Walls");

        makePatrolPath();
    }

    // Update is called once per frame
    void Update()
    {
        tree.Reset();
        tree.Tick();

        Debug.DrawLine(transform.position, transform.position + transform.up, Color.white ,0.1f);
    }

    void makePatrolPath()
    {
        patrolPath = new GameObject[3];
        patrolPath[0] = new GameObject();
        patrolPath[0].transform.position = new Vector2(-5, 1);
        patrolPath[1] = new GameObject();
        patrolPath[1].transform.position = new Vector2(0, -5);
        patrolPath[2] = new GameObject();
        patrolPath[2].transform.position = new Vector2(1, 0);
    }

    [Task]
    public bool IsPlayerVisible()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position-transform.position, viewDistance, mask);
        float angle = Vector2.Angle(transform.up, player.transform.position-transform.position);

        bool canSee = hit.transform.tag == "Player" && angle < viewAngle;
        if (canSee)
        {
            if(lastKnownPlayerPos == null) lastKnownPlayerPos = new GameObject("lastKnownPlayerPos");
            lastKnownPlayerPos.transform.position = player.transform.position;
        }

        return canSee;
    }

    [Task]
    public void Attack()
    {
        pathPlanner.maxSpeed = 0.03f;
        pathPlanner.rotationSpeed = 0;

        Quaternion rotation = Quaternion.LookRotation(player.transform.forward, player.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 8);
        // fier!   
    }

    [Task]
    public bool PlayerPositionKnown()
    {
        if (lastKnownPlayerPos != null)
        {
            if (Vector2.Distance(transform.position, lastKnownPlayerPos.transform.position) < 0.1)
                Destroy(lastKnownPlayerPos);
        }

        return lastKnownPlayerPos != null;
    }

    [Task]
    public void ChasePlayer()
    {
        destinationsetter.target = lastKnownPlayerPos.transform;
        pathPlanner.maxSpeed = 3;
        pathPlanner.rotationSpeed = 600;
    }


    [Task]
    public void Patrolling()
    {
        destinationsetter.target = patrolPath[waypoint].transform;
        pathPlanner.maxSpeed = 1;
        pathPlanner.rotationSpeed = 360;

        if (Vector2.Distance(transform.position, patrolPath[waypoint].transform.position) < 1)
            waypoint = (waypoint + 1) % patrolPath.Length;
    }
}
