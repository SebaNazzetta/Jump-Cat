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
        if (_checkpoint != null)
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
        var paraboleForce = 1f;
        var paraboleVariance = Random.Range(1f, 2f);
        var particleObj = player.GetComponent<PlayerController>()
            .particleOnDeath;
        var playerSR = player.GetComponentInChildren<SpriteRenderer>();

        particleObj.SetActive(true);
        playerSR.enabled = false;

        Vector3 startPosition = player.transform.position;

        bool goingRight = lastCheckpointPosition.x > startPosition.x;

        var curveMove = goingRight ?
            -1 * paraboleForce * paraboleVariance :
            +1 * paraboleForce * paraboleVariance;

        var t = 0f;
        while (t < _timeMoving)
        {
            t += Time.deltaTime;
            var elapsedTime = t / _timeMoving;

            var y = Mathf.Lerp(startPosition.y, lastCheckpointPosition.y, 
                elapsedTime);

            var x = Mathf.Lerp(startPosition.x, lastCheckpointPosition.x, 
                elapsedTime);
            x += curveMove * Mathf.Sin(elapsedTime * Mathf.PI);

            player.transform.position = new Vector3(x, y, 0);
            yield return null;
        }

        player.transform.position = lastCheckpointPosition;

        yield return new WaitForSeconds(0.5f);

        playerSR.enabled = true;

        yield return new WaitForSeconds(1f);
        particleObj.SetActive(false);
    }
}