using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;

public class SpiderEyeController : MonoBehaviour
{
    [SerializeField]
    GameObject[] leftLegs;

    [SerializeField] 
    GameObject[] rightLegs;

    [SerializeField]
    Transform[] leftTargets;

    [SerializeField]
    Transform[] rightTargets;

    [SerializeField]
    Transform player;

    [SerializeField]
    Transform[] legTargetHeightSetter;

    [SerializeField]
    Transform[] allLegTargets;

    [SerializeField]
    float legRange;

    [SerializeField]
    float multiplier;

    [SerializeField]
    float legSpeed;

    [SerializeField]
    AIDestinationSetter destinationSetter;

    [SerializeField]
    LayerMask whatIsGround;
    public bool debugFollowPlayer;

    float timer;

    GameObject playerObj;

    private int leftLegIndex = 1;

    private int rightLegIndex = 1;

    int targetIndex = 0;

    bool lefttLegMoved;
    bool hasRunOnRightLeg = false;
    bool hasRunOnLeftLeg = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       playerObj = GameObject.FindGameObjectWithTag("Player");
       player = playerObj.transform;
       destinationSetter.target = player;
    }

    private void Update()
    {
        if (debugFollowPlayer)
        {
            this.transform.position = new Vector3(player.transform.position.x, this.transform.position.y);
        }

        timer += Time.deltaTime;


        RaycastHit2D height = Physics2D.Raycast(legTargetHeightSetter[targetIndex].position ,Vector2.down, 3f, whatIsGround);

        Debug.Log(height.point);

        if (height.collider != null)
        {
            allLegTargets[targetIndex].position = height.point;
        }
        
        targetIndex++;

        if (targetIndex == 4)
        {
            targetIndex = 0;
        }

      
      


        MoveLegs();
        
        
    }

    private void OnDrawGizmos()
    {
        Vector2 spiderRotation = this.transform.rotation.eulerAngles;

        Debug.DrawRay(legTargetHeightSetter[leftLegIndex].position,Vector2.down * 3,Color.green, 0.1f);
        
    }

    private void MoveLegs()
    {
        if ((leftLegs[leftLegIndex].transform.position.x > leftTargets[leftLegIndex].transform.position.x + legRange || leftLegs[leftLegIndex].transform.position.x < leftTargets[leftLegIndex].position.x - legRange) && !lefttLegMoved && !hasRunOnLeftLeg)
        {
            LeanTween.move(leftLegs[leftLegIndex], new Vector2((leftTargets[leftLegIndex].position.x + leftLegs[leftLegIndex].transform.position.x) / 2, leftLegs[leftLegIndex].transform.position.y + 1.5f), legSpeed);
            hasRunOnLeftLeg = true;

        }

        if (hasRunOnLeftLeg && timer >= legSpeed)
        {
            LeanTween.move(leftLegs[leftLegIndex], leftTargets[leftLegIndex].position, legSpeed);
            leftLegIndex = leftLegIndex + 1;
            lefttLegMoved = true;
            hasRunOnLeftLeg = false;
            timer = 0f;
        }

        if (leftLegIndex == 2)
        {
            leftLegIndex = 0;
        }




        if ((rightLegs[rightLegIndex].transform.position.x > rightTargets[rightLegIndex].position.x + legRange || rightLegs[rightLegIndex].transform.position.x < rightTargets[rightLegIndex].position.x - legRange) && lefttLegMoved && !hasRunOnRightLeg)
        {
            LeanTween.move(rightLegs[rightLegIndex], new Vector2((rightTargets[rightLegIndex].position.x + rightLegs[rightLegIndex].transform.position.x) / 2, rightLegs[rightLegIndex].transform.position.y + 1.5f), legSpeed);
            hasRunOnRightLeg = true;

        }

        if (hasRunOnRightLeg && timer >= legSpeed)
        {
            LeanTween.move(rightLegs[rightLegIndex], rightTargets[rightLegIndex].position, legSpeed);
            rightLegIndex = rightLegIndex + 1;
            lefttLegMoved = false;
            hasRunOnRightLeg = false;
            timer = 0f;
        }

        if (rightLegIndex == 2)
        {
            rightLegIndex = 0;
        }
    }


}
