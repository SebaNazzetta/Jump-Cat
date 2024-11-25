using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public GameObject player;
    public Vector3 lastCheckpoint;

    private void Start()
    {
        player = GameObject.FindObjectOfType<PlayerController>().gameObject;
    }

    public void SaveCurrentPosition()
    {
        lastCheckpoint = player.transform.position;
    }

    public void LoadLastCheckpoint()
    {
        player.transform.position = lastCheckpoint;
    }
}