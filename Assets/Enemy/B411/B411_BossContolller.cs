using System.Collections;
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

    [SerializeField]
    public PlayerController playerController;


    Transform target;

    float timer;

    public bool isAttacking;
    
    public bool hasRun;
    public bool hasRun2;
    public int attack;
    public int lastNumber = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        //StartCoroutine(Leap());
        //StartCoroutine(Slash());

    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        StartCoroutine(Choose());
    }

    IEnumerator Choose()
    {
        attack = Random.Range(1, 4);
        if (lastNumber == attack)
        {
            StartCoroutine(Choose());
        }
        else
        {
            lastNumber = attack;
            StartCoroutine(Attack());
            yield return null;
        }
        
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.25f);
        if (attack == 1)
        {
            StartCoroutine(Leap());
        }
        else if (attack == 2)
        {
            StartCoroutine(Slash());
        }
        else if(attack == 3)
        {
            StartCoroutine(Summon());
        }
        yield return null;
    }

    private void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!isAttacking)
        {
            Rotate();
        }

    }


    IEnumerator Leap()
    {
        isAttacking = true;

            LeanTween.move(this.gameObject, new Vector2(this.transform.position.x, this.transform.position.y + leapHeight), leaptime);

        yield return new WaitForSeconds(leaptime);
        Rotate();
            target = player.transform;
            LeanTween.move(this.gameObject, new Vector2(target.position.x, target.position.y + 1), leaptime);

        yield return new WaitForSeconds(leaptime);
        isAttacking = false;

        StartCoroutine(Choose());
        yield return null;
    }

    IEnumerator Slash()
    {
        isAttacking = true;
        swing.SetActive(true);
        swing.transform.rotation = Quaternion.Euler(0f, 0f, 119.439f);
        swingAnim.Play("Swing");

        yield return new WaitForSeconds(0.75f);
        playerController.virtualCameraPerlin.m_AmplitudeGain = 20;
        yield return new WaitForSeconds(0.2f);
        

        swing.SetActive(false);
            isAttacking = false;
        StartCoroutine(Choose());
        yield return new WaitForSeconds(0.2f);
        playerController.virtualCameraPerlin.m_AmplitudeGain = 0;
        yield return null;
    }

    private void Away()
    {

    }

    private void Rotate()
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


    float playerX;

    IEnumerator Summon()
    {
       
            playerX = player.transform.position.x;
            timer = 0;
            Instantiate(sword, new Vector2(playerX, player.transform.position.y + 19.6f), Quaternion.Euler(0, 0, -90));

        
        yield return new WaitForSeconds(0.25f);
       
            Instantiate(sword, new Vector2(playerX - 4, player.transform.position.y + 19.6f), Quaternion.Euler(0, 0, -90));
            Instantiate(sword, new Vector2(playerX + 4, player.transform.position.y + 19.6f), Quaternion.Euler(0, 0, -90));
        
        StartCoroutine(Choose());
        yield return null;
    }
}
