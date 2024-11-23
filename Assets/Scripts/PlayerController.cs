using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _lateralForce = 6;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _jumpValue = 0.0f;
    [SerializeField] private float _maxJumpValue = 10;
    [SerializeField, Range(0, 10f)] private float _bounceForce = 1f;
    [SerializeField] private ButtonPressed _leftButton;
    [SerializeField] private ButtonPressed _rightButton;
    private bool _isGrounded
    {
        get => _playerCollision.IsGrounded();
    }
    private bool _hasWallInFront
    {
        get => _playerCollision.HasWallInFront();
    }
    private Rigidbody2D _rb;
    private bool _isJumping = false;
    private Animator _anim;
    private PlayerCollision _playerCollision;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();
        _playerCollision = GetComponent<PlayerCollision>();
    }

    void Update()
    {
        CheckButtonPressed();

        if (_isGrounded && _jumpValue == 0)
        {
            _rb.velocity = new Vector2(0, 0);
        }

        if (_rb.velocity.y != 0 && _hasWallInFront)
        {
            _rb.velocity = new Vector2(-_bounceForce * _rb.velocity.x,
                _rb.velocity.y);

            transform.localScale = new Vector3(-1 * transform.localScale.x,
                1, 1);
        }

        if (_jumpValue >= _maxJumpValue && _isGrounded)
        {
            float tempx = this.transform.localScale.x * _lateralForce;
            float tempy = _jumpValue;
            _rb.velocity = new Vector2(tempx, tempy);
            _anim.SetBool("isPreJumping", false);
            Invoke("ResetJump", 0.2f);
        }

        if (_isJumping)
        {
            if (_isGrounded)
            {
                _rb.velocity = new Vector2(this.transform.localScale.x * _lateralForce, _jumpValue);
                _jumpValue = 0.0f;
                _isJumping = false;
                _anim.SetBool("isPreJumping", false);
            }
        }
        _anim.SetFloat("VerticalVelocity", _rb.velocity.y);
    }

    void ResetJump()
    {
        _jumpValue = 0;
    }

    public void CheckButtonPressed()
    {
        if (_leftButton.buttonPressed && _isGrounded)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            _jumpValue += 0.1f;
            _anim.SetBool("isPreJumping", true);
            return;
        }
        else if (_rightButton.buttonPressed && _isGrounded)
        {
            transform.localScale = new Vector3(1, 1, 1);
            _jumpValue += 0.1f;
            _anim.SetBool("isPreJumping", true);
            return;
        }

        if (!_leftButton.buttonPressed && !_rightButton.buttonPressed)
        {

            _anim.SetBool("isPreJumping", false);
            if (_jumpValue > 0)
            {
                _isJumping = true;
            }
        }
    }

    /*
#if UNITY_EDITOR

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.55f), new Vector2(0.4f, 0.1f));
    }
#endif
    */
}
