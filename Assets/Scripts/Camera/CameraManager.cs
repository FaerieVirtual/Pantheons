using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    private CinemachineVirtualCamera vcam;
    void Start()
    {
        vcam = gameObject.GetComponent<CinemachineVirtualCamera>();
        vcam.Follow = FindObjectOfType<PlayerManager>().transform;
    }
    private void FixedUpdate()
    {
        if (vcam.Follow == null)
        {
            vcam.Follow = FindObjectOfType<PlayerManager>().transform;
        }
    }
}
