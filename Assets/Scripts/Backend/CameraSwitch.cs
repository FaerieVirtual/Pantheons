using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    private CinemachineVirtualCamera vcam;
    void Start()
    {
        var vcam = GetComponent<CinemachineVirtualCamera>();
        vcam.Follow = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
