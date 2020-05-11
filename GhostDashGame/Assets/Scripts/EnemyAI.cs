using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{

    GameObject player;
    PandaBehaviour tree;
    Navigator navigator;

    GameObject lastKnownPlayerPos;
    GameObject[] patrolPath;
    int waypoint = 0;

    float hearingDistance = 2;
    float viewAngle = 70;       // view angle per side
    float viewDistance = 100;
    LayerMask mask;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        tree = GetComponent<PandaBehaviour>();
        navigator = GetComponent<Navigator>();

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
        patrolPath[0].transform.position = new Vector2(-8, 1);
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
    public bool IsPlayerColse()
    {
        return Vector2.Distance(transform.position, player.transform.position) < hearingDistance;
    }

    [Task]
    public void LookAtPlayer()
    {
        navigator.navigate = false; // stop moving

        Quaternion rotation = Quaternion.LookRotation(player.transform.forward, player.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 8);
    }

    [Task]
    public bool PlayerPositionKnown()
    {
        if (lastKnownPlayerPos != null)
        {
            if (Vector2.Distance(transform.position, lastKnownPlayerPos.transform.position) < 0.5)
                Destroy(lastKnownPlayerPos);
        }

        return lastKnownPlayerPos != null;
    }

    [Task]
    public void ChasePlayer()
    {
        navigator.navigate = true;
        navigator.target = lastKnownPlayerPos.transform;
        navigator.speed = 150;
    }


    [Task]
    public void Patrolling()
    {
        navigator.navigate = true;
        navigator.target = patrolPath[waypoint].transform;
        navigator.speed = 50;

        if (Vector2.Distance(transform.position, patrolPath[waypoint].transform.position) < 1)
            waypoint = (waypoint + 1) % patrolPath.Length;
    }
}
