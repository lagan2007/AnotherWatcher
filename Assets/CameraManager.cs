using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [SerializeField] private CinemachineVirtualCamera[] allVirtualCameras;

    [SerializeField]
    private float fallPanAmount = 0.25f;
    [SerializeField]
    private float fallPanTime = 0.35f;

    public float fallSpeedDampingChangeTreshold = -15f;

    public bool IsLerpingYDamping {  get; private set; }

    public bool LerpedFromLayerFalling { get; set; }

    private Coroutine lerpYPanCorutine;

    private CinemachineVirtualCamera currentCamera;
    private CinemachineFramingTransposer framingTransposer;

    private float normYPanAmount;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        for (int i = 0; i < allVirtualCameras.Length; i++)
        {

            //set the current active camera
            currentCamera = allVirtualCameras[i];

            //set the framing transposer
            framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        }

        //set the YDamping amount so its based of the inspector value
        normYPanAmount = framingTransposer.m_YDamping;
    }

    #region Lerp the Y Damping

    public void LerpYDamping(bool isPlayerFalling)
    {
        lerpYPanCorutine = StartCoroutine(LerpYAction(isPlayerFalling));
    }

    private IEnumerator LerpYAction(bool isPlayerFalling)
    {
        IsLerpingYDamping = true;

        //grab the starting damping amount
        float startDampAmount = framingTransposer.m_YDamping;
        float endDampAmount = 0;

        //determine the end daming amount
        if (isPlayerFalling)
        {
            endDampAmount = fallPanAmount;
            LerpedFromLayerFalling = true;
        }
        else
        {
            endDampAmount = normYPanAmount;
        }

        //lerp the pan amount
        float elapsedTime = 0f;
        while (elapsedTime < fallPanTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, (elapsedTime / fallPanTime));
            framingTransposer.m_YDamping = lerpedPanAmount;

            yield return null;

        }

        IsLerpingYDamping = false;

    }

    #endregion
}
