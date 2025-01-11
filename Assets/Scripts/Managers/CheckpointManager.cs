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
    [Tooltip("Time to move the player to the checkpoint")]
    [SerializeField, Range(0.1f, 10)] private float _timeMoving = 1f;

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
        StartCoroutine(MovePlayerToCheckpoint());
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

    private IEnumerator MovePlayerToCheckpoint()
    {
        var particleObj = player.GetComponent<PlayerController>()
            .particleOnDeath;
        var playerSR = player.GetComponentInChildren<SpriteRenderer>();

        particleObj.SetActive(true);
        playerSR.enabled = false;

        var t = 0f;
        Vector3 lastPosition = player.transform.position;

        //Agregar un vector 3 con el x modificado para la parabola y hacer un lerp

        while(t < _timeMoving)
        {
            t += Time.deltaTime;
            player.transform.position = Vector3.Lerp(lastPosition, 
                lastCheckpointPosition, t / _timeMoving);
            yield return null;
        }


        player.transform.position = lastCheckpointPosition;

        yield return new WaitForSeconds(0.75f);

        playerSR.enabled = true;
        particleObj.SetActive(false);
    }
}