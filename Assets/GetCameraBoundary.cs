using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;
using System.Reflection;

public class GetCameraBoundary : MonoBehaviour
{
    private GameObject gameObjectCameraBoundary;
    public CompositeCollider2D cameraBoundary;
    [SerializeField]
    private CinemachineConfiner2D confiner;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObjectCameraBoundary = GameObject.FindGameObjectWithTag("CameraBoundary");
        cameraBoundary = gameObjectCameraBoundary.GetComponent<CompositeCollider2D>();

        confiner.m_BoundingShape2D = cameraBoundary;
    }

    private void Update()
    {
        if (gameObjectCameraBoundary == null)
        {
            gameObjectCameraBoundary = GameObject.FindGameObjectWithTag("CameraBoundary");
            cameraBoundary = gameObjectCameraBoundary.GetComponent<CompositeCollider2D>();

            confiner.m_BoundingShape2D = cameraBoundary;
        }
        

    }

}
