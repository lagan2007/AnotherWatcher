using UnityEngine;

public class FallingBlade : MonoBehaviour
{

    [SerializeField]
    Rigidbody2D body;

    [SerializeField]
    float yPositionToStop;

    float timer;
    float timer2;
    bool fallen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        timer = timer + Time.deltaTime;
        if(timer <= 0.5 && !(this.transform.position.y <= yPositionToStop))
        {
            body.gravityScale = 0;
            body.linearVelocity = new Vector2(0,0);
        }

        if(timer >= 0.5 && !(this.transform.position.y <= yPositionToStop))
        {
            body.gravityScale = 11f;
        }

        if (this.transform.position.y <= yPositionToStop)
        {
            timer2 = timer2 + Time.deltaTime;
            body.gravityScale = 0;
            body.linearVelocity = Vector2.down * 0;
        }

        if (timer2 >= 10.75f)
        {
            Object.Destroy(this.gameObject);
        }
    }
}
