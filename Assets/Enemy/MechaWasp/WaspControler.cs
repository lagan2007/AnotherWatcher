using Pathfinding;
using Pathfinding.Util;
using UnityEngine;
using UnityEngine.AI;

public class WaspControler : MonoBehaviour
{
    [SerializeField]
    AIDestinationSetter AIDestinationSetter;
    [SerializeField]
    AIPath path;
    [SerializeField]
    Transform player;
    [SerializeField]
    float visionRange;

   


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.transform.position.x < player.position.x)
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        if (this.transform.position.x > player.position.x)
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void FixedUpdate()
    {
        RaycastHit2D vision = Physics2D.Raycast(this.transform.position, player.position - this.transform.position, visionRange);
        
        if (vision)
        {
            bool seesPlayer = vision.collider.CompareTag("Player");
            if (seesPlayer)
            {
                Debug.Log("I see you");
                path.canMove = true;
                Debug.DrawRay(this.transform.position, player.position - this.transform.position, Color.green, 0.1f);
                if (AIDestinationSetter.target == null)
                {
                    AIDestinationSetter.target = player;
                }
            }
            else
            {
                AIDestinationSetter.target = null;
                Debug.Log("hes gone");
                Debug.DrawRay(this.transform.position, player.position - this.transform.position, Color.red, 0.1f);
                path.canMove = false;
            }
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, visionRange);
    }

}
