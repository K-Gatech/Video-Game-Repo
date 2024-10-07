using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AlienAI : MonoBehaviour
{
    // Start is called before the first frame update

    public enum AIState
    {
        FreeRoam,
        Pursue,
        Observe,
        WaypointNavigation
    };

    public AIState aiState;
    public bool aggressive, waypointsEnabled;

    public GameObject player;
    public GameObject[] waypoints;
    public GameObject projectilePrefab;

    private NavMeshAgent navMeshAgent;
    private float pathTimerLimit, pathTimer, aggressionTimer, aggressionTimerLimit, projectileTimer, projectTileCooldown;
    private float pursuitSpeedScaling;
    private int walkRadius, consecutiveTimer, nextWaypoint, fovAngle;
    private Vector3 targetLook;
    private Animator anim;
    private bool checkFOV;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        pathTimerLimit = 10f;
        aggressionTimerLimit = 30.0f;
        projectTileCooldown = 0.2f;
        pathTimer = 0f;
        walkRadius = 10;
        consecutiveTimer = 0;
        aggressionTimer = 0;
        pursuitSpeedScaling = 1.0f;
        nextWaypoint = -1;
        fovAngle = 90;
        checkFOV = false;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        Vector3 thisPos = new Vector3(this.transform.position.x, 0, this.transform.position.z);
        Debug.DrawLine(playerPos, thisPos);

        switch(aiState)
        {
            case AIState.FreeRoam:
                pathTimer += Time.deltaTime;
                if((navMeshAgent.remainingDistance == 0 && !navMeshAgent.pathPending) || pathTimer >= pathTimerLimit)
                {
                    FreeRoam();
                    if(pathTimer >= pathTimerLimit)
                        consecutiveTimer++;
                }
                break;
            
            case AIState.Observe:
                Observe();
                break;

            case AIState.Pursue:
                Pursue();
                CheckPursuit();
                break;
            
            case AIState.WaypointNavigation:
                if(navMeshAgent.remainingDistance == 0 && !navMeshAgent.pathPending)
                {
                    WaypointNavigation();
                }
                break;
        }

        if(checkFOV)
        {
            PlayerInFOV();
        }

        SetAnimation();
    }

    void FreeRoam()
    {
        // Core logic has been taken from Unity forums: https://answers.unity.com/questions/475066/how-to-get-a-random-point-on-navmesh.html

        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
        Vector3 finalPosition = hit.position;

        //end of citation

        if(consecutiveTimer > 1)
            finalPosition = Quaternion.Euler(0, 180, 0) * finalPosition;

        navMeshAgent.SetDestination(finalPosition);
        pathTimer = 0f;
        consecutiveTimer = 0;
    }

    void Observe()
    {
        navMeshAgent.SetDestination(this.transform.position);
        targetLook = new Vector3(player.transform.position.x,
                                this.transform.position.y,
                                player.transform.position.z);
        this.transform.LookAt(targetLook);
        
        // if(aggressive)
        // {
        //     aggressionTimer++;
        //     if(aggressionTimer >= aggressionTimerLimit)
        //         aiState = AIState.Pursue;
        // }
    }

    void Pursue()
    {
        navMeshAgent.speed = pursuitSpeedScaling * navMeshAgent.speed;
        Vector3 futureTarget = player.transform.position;

        navMeshAgent.CalculatePath(futureTarget, navMeshAgent.path);
        bool pathComplete = true;

        if(!pathComplete)
        {
            if(navMeshAgent.path.status == NavMeshPathStatus.PathComplete || navMeshAgent.hasPath && navMeshAgent.path.status == NavMeshPathStatus.PathPartial)
            {
                navMeshAgent.SetDestination(futureTarget);
            }
        }
        else
            navMeshAgent.SetDestination(futureTarget);

        targetLook = new Vector3(futureTarget.x,
                                this.transform.position.y,
                                futureTarget.z);
        this.transform.LookAt(targetLook);
    }

    void WaypointNavigation()
    {
        nextWaypoint = (nextWaypoint + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[nextWaypoint].transform.position);
    }

    void CheckPursuit()
    {
        if((player.transform.position-this.transform.position).magnitude >= aggressionTimerLimit)
        {
            navMeshAgent.speed = navMeshAgent.speed / pursuitSpeedScaling;
            if(!waypointsEnabled)
            {
                aiState = AIState.FreeRoam;
            }
            else
            {
                navMeshAgent.ResetPath();
                aiState = AIState.WaypointNavigation;    
            }
        }
    }

    void PlayerInFOV()
    {
        Vector3 playerPos = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        Vector3 thisPos = new Vector3(this.transform.position.x, 0, this.transform.position.z);
        Vector3 playerDirection =  playerPos - thisPos;
        // playerDirection.Normalize();
        float playerAngle = Vector3.Angle(playerDirection, this.transform.forward);

        Debug.Log(playerAngle);

        if(playerAngle <= fovAngle)   // change condition
        {
            if(!aggressive)
                aiState = AIState.Observe;
            else
                aiState = AIState.Pursue;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && aiState != AIState.Pursue)
        {
            checkFOV = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(aiState != AIState.Pursue)
            {
                if(!waypointsEnabled)
                {
                    aiState = AIState.FreeRoam;
                }
                else
                {
                    navMeshAgent.ResetPath();
                    aiState = AIState.WaypointNavigation;    
                }
            }
            checkFOV = false;
        } 
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Projectile"))
        {
            aiState = AIState.Pursue;
        }
    }

    void SetAnimation()
    {
        if(this.gameObject.tag == "Sentry")
        {
            if(aiState == AIState.Pursue)
            {
                Light trackingLight = this.transform.Find("Tracking Light").GetComponent<Light>();
                trackingLight.color = Color.red;
                if(projectileTimer > projectTileCooldown)
                {
                    Vector3 playerAim = new Vector3(player.transform.position.x, player.transform.position.y+2, player.transform.position.z);
                    Vector3 objLoc = new Vector3(this.transform.position.x, player.transform.position.y+2, this.transform.position.z+1);
                    Vector3 aimDir = playerAim - objLoc;
                    Instantiate(projectilePrefab, objLoc, Quaternion.LookRotation(aimDir, Vector3.up));
                    projectileTimer = 0.0f;
                }
                else
                    projectileTimer += Time.deltaTime;
            }
            else
            {
                Light trackingLight = this.transform.Find("Tracking Light").GetComponent<Light>();
                trackingLight.color = Color.green;
                projectileTimer = 0.0f;
            }
        }
        else if(this.gameObject.tag == "GreenNPC")
        {
            if(aiState == AIState.Observe)
            {
                // Play some animation
                anim.SetFloat("Walk", 0);
            }
            else if(aiState == AIState.FreeRoam)
            {
                // Play some animation
                anim.SetFloat("Walk", 1);
            }
        }
    }
}
