using UnityEngine;

public class FallingBlade : MonoBehaviour
{

    [SerializeField]
    Rigidbody2D body;

    [SerializeField]
    Collider2D collider;

    [SerializeField]
    SpriteRenderer spriteRenderer;

    [SerializeField]
    Sprite brokenBlade;

    [SerializeField]
    GameObject sword;

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
        DropSwords();
    }

    private void DropSwords()
    {
        //hold sword in air
        timer = timer + Time.deltaTime;
        if (timer <= 0.5 && !(this.transform.position.y <= yPositionToStop))
        {
            body.gravityScale = 0;
            body.linearVelocity = new Vector2(0, 0);

        }

        //drop sword
        if (timer >= 0.5 && !(this.transform.position.y <= yPositionToStop))
        {
            body.gravityScale = 11f;
        }


        //hold sword in ground
        if (this.transform.position.y <= yPositionToStop)
        {
            timer2 = timer2 + Time.deltaTime;
            body.gravityScale = 0;
            body.linearVelocity = Vector2.down * 0;

            collider.enabled = false;
            spriteRenderer.sprite = brokenBlade;
            sword.transform.localPosition = new Vector2(sword.transform.localPosition.x, 0);

        }

        if (timer2 >= 3)
        {
            Object.Destroy(this.gameObject);
        }
    }
}
