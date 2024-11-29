using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckpointManager : MonoBehaviour
{
    public GameObject player;
    public bool checkpointsActive = true;
    public Vector3 lastCheckpointPosition;
    private Checkpoint _checkpoint;
    public GameObject checkpointUI;

    private void Start()
    {
        player = GameObject.FindObjectOfType<PlayerController>().gameObject;
        CloseCheckpointUI();
    }

    public void SaveCurrentPosition()
    {
        //ACAAAAA VAA LAA ADDD
        //ACAAAAA VAA LAA ADDD
        //ACAAAAA VAA LAA ADDD
        lastCheckpointPosition = player.transform.position;
        _checkpoint.SetCurrentCheckpoint();
        CloseCheckpointUI();
    }

    public void LoadLastCheckpoint()
    {
        player.transform.position = lastCheckpointPosition;
    }

    public void OpenCheckpointUI(Checkpoint current)
    {
        checkpointUI.SetActive(true);
        _checkpoint = current;
    }

    public void CloseCheckpointUI()
    {
        checkpointUI.SetActive(false);
    }
}