using UnityEngine;

public class DocWeaponVisuals : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]
    SpriteRenderer renderer;

    [SerializeField]
    Transform playerTransform;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform.rotation.y == 0f) 
        { 
            renderer.sortingOrder = -1;
        }
        else
        {
            renderer.sortingOrder = 0;
        }
    }
}
