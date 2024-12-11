using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool _isCurrentCheckpoint;
    private PlayerCollision _playerCollision;
    private BoxCollider2D _boxCollider2D;
    [SerializeField] private Sprite _deactivatedSprite;
    [SerializeField] private Sprite _activatedSprite;

    private CheckpointManager _checkpointManager;
    private bool _isPlayerGrounded
    {
        get => _playerCollision.IsGrounded();
    }
    private void Awake()
    {
        _playerCollision = FindObjectOfType<PlayerCollision>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _checkpointManager = FindObjectOfType<CheckpointManager>();

    }
    private void Update()
    {
        if (_isPlayerGrounded)
        {
            _boxCollider2D.enabled = true;
            return;
        }
        _boxCollider2D.enabled = false;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !_isCurrentCheckpoint)
        {
            //Open UI for checkpoints
            _checkpointManager.OpenCheckpointUI(this);
        }
    }

    public void SetCurrentCheckpoint()
    {
        foreach (Checkpoint checkpoint in FindObjectsOfType<Checkpoint>())
        {
            checkpoint._isCurrentCheckpoint = false;
            checkpoint.GetComponent<SpriteRenderer>().sprite = checkpoint._deactivatedSprite;
            if (checkpoint == this)
            {
                checkpoint.ActivateCheckpoint();
                Vector2 checkpointPosition = checkpoint.transform.position;
                //Save the last checkpoint position in PlayerPrefs
                string x = checkpointPosition.x.ToString(CultureInfo.InvariantCulture);
                string y = checkpointPosition.y.ToString(CultureInfo.InvariantCulture);
                PlayerPrefs.SetString("LastCheckpoint", $"{x};{y}");
                PlayerPrefs.Save();

            }
        }
    }

    public void ActivateCheckpoint()
    {
        _isCurrentCheckpoint = true;
        GetComponent<SpriteRenderer>().sprite = _activatedSprite;
    }
}
