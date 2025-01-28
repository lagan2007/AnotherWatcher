using Pathfinding;
using Pathfinding.Util;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class WaspControler : MonoBehaviour
{
    [SerializeField]
    AIDestinationSetter AIDestinationSetter;
    [SerializeField]
    AIPath path;
    [SerializeField]
    Transform playerTransform;
    [SerializeField]
    float visionRange;
    [SerializeField]
    float maxTimer;

    [SerializeField]
    private LayerMask noEnemyLayerMask;

    [SerializeField]
    GameObject[] waypoints;

    public int waypointIndex;
    public int waypointCount;

    public bool seesPlayer;


    bool hasRunOnRoam = false;

    GameObject playerObj;
    private float lastPosX;
    public float timer;

   


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.transform.position = waypoints[0].transform.position;
        waypointCount = waypoints.Length;
    }

    // Update is called once per frame
    void Update()
    {
        

        if(this.transform.position.x > lastPosX)
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
            lastPosX = this.transform.position.x;
        }
        if (this.transform.position.x < lastPosX)
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
            lastPosX = this.transform.position.x;
        }
    }

    private void FixedUpdate()
    {
        if (playerTransform == null)
        {
            playerObj = GameObject.FindGameObjectWithTag("Player");
            playerTransform = playerObj.transform;
        }

        RaycastHit2D vision = Physics2D.Raycast(this.transform.position, playerTransform.position - this.transform.position, visionRange, noEnemyLayerMask);

        Debug.DrawRay(this.transform.position, playerTransform.position - this.transform.position, Color.red, 0.01f);

        if (vision.collider != null)
        {
            seesPlayer = vision.collider.CompareTag("Player");
        }
        else
        {
            seesPlayer = false;
        }

        if (seesPlayer)
        {
            Debug.Log("I see you");
            path.canMove = true;
            Debug.DrawRay(this.transform.position, playerTransform.position - this.transform.position, Color.yellow, 0.01f);
            timer = maxTimer;

            if (AIDestinationSetter.target != playerTransform)
            {
                AIDestinationSetter.target = playerTransform;
            }
                
        }

        if(waypointIndex + 1 >= waypointCount)
        {
            //waypointIndex = 0;
        }

        if(!seesPlayer)
        {
            timer -= Time.deltaTime;
            
            //Debug.Log("hes gone");
            
            if (timer <= 0)
            {
                AIDestinationSetter.target = waypoints[waypointIndex].transform;

                Roam();

                //path.canMove = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //path.canMove = false;
        }
    }

    private void Roam()
    {
        if (path.reachedDestination && !hasRunOnRoam && AIDestinationSetter.target == waypoints[waypointIndex].transform)
        {
            hasRunOnRoam = true;
            waypointIndex = waypointIndex + 1;
            if (waypointIndex + 1 > waypointCount)
            {
                waypointIndex = 0;
            }
            AIDestinationSetter.target = waypoints[waypointIndex].transform;
            hasRunOnRoam = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, visionRange);
    }

}
