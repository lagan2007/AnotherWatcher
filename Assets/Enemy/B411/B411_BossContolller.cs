using UnityEngine;

public class B411_BossContolller : MonoBehaviour
{

    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject swing;
    [SerializeField]
    GameObject sword;

    [SerializeField]
    Animator animator;

    [SerializeField]
    Animator swingAnim;

    [SerializeField]
    float leapHeight;
    [SerializeField]
    float leaptime;

    Transform target;

    float timer;

    public bool isAttacking;
    
    public bool hasRun;
    public bool hasRun2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //swing.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        timer = timer + Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        //Leap();
        //Slash();
        Summon();

    }

    private void Leap()
    {
        isAttacking = true;
        if (!hasRun)
        {
            LeanTween.move(this.gameObject, new Vector2(this.transform.position.x, this.transform.position.y + leapHeight), leaptime);

            hasRun = true;
        }

        if (!LeanTween.isTweening(this.gameObject) && !hasRun2)
        {
            target = player.transform;
            LeanTween.move(this.gameObject,new Vector2(target.position.x, target.position.y + 1), leaptime);

            hasRun2 = true;
            isAttacking = false;
        }

    }

    private void Slash()
    {
        isAttacking = true;
        swing.SetActive (true);
        swing.transform.rotation = Quaternion.Euler(0f, 0f, 119.439f);
        swingAnim.Play("Swing");
        if (swingAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !swingAnim.IsInTransition(0))
        {
            swing.SetActive (false);
            isAttacking = false;
        }
        

    }

    private void Away()
    {

    }

    private void Rotate()
    {
        if (!isAttacking)
        {
            if (player.transform.position.x > this.transform.position.x)
            {
                this.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }

            if (player.transform.position.x < this.transform.position.x)
            {
                this.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
        }
        
    }


    float playerX;
    private void Summon()
    {
        if (!hasRun)
        {
            playerX = player.transform.position.x;
            timer = 0;
            Instantiate(sword, new Vector2(playerX, player.transform.position.y + 19.6f), Quaternion.Euler(0, 0, -90));
            hasRun = true;
        }

        if(!hasRun2 && timer >= 0.25)
        {
            Instantiate(sword, new Vector2(playerX - 4, player.transform.position.y + 19.6f), Quaternion.Euler(0, 0, -90));
            Instantiate(sword, new Vector2(playerX + 4, player.transform.position.y + 19.6f), Quaternion.Euler(0, 0, -90));
            hasRun2 = true;
        }
    }
}
