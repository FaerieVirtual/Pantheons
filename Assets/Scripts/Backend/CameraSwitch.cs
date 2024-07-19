using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    private CinemachineVirtualCamera vcam;
    void Start()
    {
        vcam = gameObject.GetComponent<CinemachineVirtualCamera>();
        vcam.Follow = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update()
    {
        if (vcam.Follow == null)
        {
            vcam.Follow = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
}
