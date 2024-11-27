using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AD_Checkpoint : MonoBehaviour
{
    private PlayerCollision _playerCollision;
    private BoxCollider2D _boxCollider2D;
    private bool _isPlayerGrounded
    {
        get => _playerCollision.IsGrounded();
    }
    private void Awake()
    {
        _playerCollision = FindObjectOfType<PlayerCollision>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
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
        if(collision.gameObject.CompareTag("Player"))
        {
            //Open UI for checkpoints
            //FindObjectOfType<CheckpointManager>().SaveCurrentPosition();
        }
    }
}
