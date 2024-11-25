using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Cinemachine.CinemachineVirtualCamera[] vcam = FindObjectsOfType<Cinemachine.CinemachineVirtualCamera>();
            foreach (Cinemachine.CinemachineVirtualCamera v in vcam)
            {
                v.Priority = 0;
            }

            this.GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 10;
        }
    }
}
