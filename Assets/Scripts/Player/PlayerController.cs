using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private bool _isPortrait = false;
    [SerializeField] private float _lateralForce = 6;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _jumpValue = 0.0f;
    [SerializeField] private float _maxJumpValue = 10;
    [SerializeField, Range(0, 10f)] private float _bounceForce = 1f;
    [SerializeField] private ButtonPressed _leftButton;
    [SerializeField] private ButtonPressed _rightButton;

    private float _timeFalling = 0;
    private float _timeHurt = 0;
    private float _timeToFall = 2f;
    private float _timeToHurt = 1f;

    private bool _isGrounded
    {
        get => _playerCollision.IsGrounded();
    }
    private bool _hasWallInFront
    {
        get => _playerCollision.HasWallInFront();
    }

    private bool _isCorner
    {
        get => _playerCollision.IsCorner();
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
        //If player falls for x seconds, then it will be hurted for x seconds
        if (!_isGrounded && _rb.velocity.y < 0)
        {
            _timeFalling += Time.deltaTime;
            if (_timeFalling >= _timeToFall)
            {
                _anim.SetBool("isHurted", true);
            }
        }
        if(_anim.GetBool("isHurted") && _isGrounded)
        {
            if(_timeHurt <= _timeToHurt)
            {
                _timeHurt += Time.deltaTime;
                return;
            }
            _anim.SetBool("isHurted", false);
            _timeHurt = 0f;
            _timeFalling = 0f;
        }

        CheckButtonPressed();
        
        //For when the player touches the ground
        if (_isGrounded && _jumpValue == 0)
        {
            _rb.velocity = new Vector2(0, 0);
        }

        //For when the player stands in a corner
        if (_isCorner && !_isGrounded)
        {
            _rb.velocity = new Vector2((_bounceForce + 0.7f) * (transform.localScale.x),
                _rb.velocity.y+0.3f);
        }

        //For when the player touches a wall ////////queda arreglar que, si saltamos hacia una pared, estando pegado a una pared, debería empujarnos de igual manera.
        if (_rb.velocity.y != 0 && _hasWallInFront)
        {
            _rb.velocity = new Vector2(-_bounceForce * _rb.velocity.x,
                _rb.velocity.y);

            transform.localScale = new Vector3(-1 * transform.localScale.x,
                1, 1);
        }

        //For when the player jumps
        if (_jumpValue >= _maxJumpValue && _isGrounded)
        {
            float tempx = this.transform.localScale.x * _lateralForce;
            float tempy = _jumpValue;
            if(_leftButton.buttonPressed && _rightButton.buttonPressed)
            {
                tempx = 0;
            }
            _rb.velocity = new Vector2(tempx, tempy);
            _anim.SetBool("isPreJumping", false);
            SetLastJumpForce(_jumpValue);
            Invoke("ResetJump", 0.2f);
        }

        //For when the player jumps at max force
        if (_isJumping)
        {
            if (_isGrounded)
            {
                _rb.velocity = new Vector2(this.transform.localScale.x * _lateralForce, _jumpValue);
                SetLastJumpForce(_jumpValue);
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
            if (_isPortrait)
            {
                _jumpValue += 0.2f;
            }
            else
            {
                _jumpValue += 0.1f;
            }
            _anim.SetBool("isPreJumping", true);
            return;
        }
        else if (_rightButton.buttonPressed && _isGrounded)
        {
            transform.localScale = new Vector3(1, 1, 1);
            if (_isPortrait)
            {
                _jumpValue += 0.2f;
            }
            else
            {
                _jumpValue += 0.1f;
            }
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

    private float _lastJumpForce;
    public void SetLastJumpForce(float force)
    {
        _lastJumpForce = force;
    }

    public float GetLastJumpForce()
    {
        return _lastJumpForce;
    }
}
