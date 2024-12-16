using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Globalization;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private bool _isPortrait = false;
    [Header("Jump Settings")]
    [SerializeField] private float _lateralForce = 6;
    [SerializeField] private float _jumpValue = 0.0f;
    [SerializeField] private float _minJumpValue = 1.5f;
    [SerializeField] private float _maxJumpValue = 10;
    [SerializeField, Range(0, 10f)] private float _bounceForce = 1f;
    [SerializeField] private PhysicsMaterial2D _bounceMaterial;

    [Header("Buttons")]
    [SerializeField] private ButtonPressed _leftButton;
    [SerializeField] private ButtonPressed _rightButton;

    private float _timeFalling = 0f;
    private float _timeHurt = 0f;
    private float _timeToFall = 2f;
    private float _timeToHurt = 1f;



    private bool _isGrounded
    {
        get => _playerCollision.IsGrounded();
    }

    private bool _isRoof
    {
        get => _playerCollision.IsRoof();
    }

    private bool _hasWallInFront
    {
        get => _playerCollision.HasWallInFront();
    }

    private bool _hasWallBehind
    {
        get => _playerCollision.HasWallBehind();
    }

    private bool _isBackCorner
    {
        get => _playerCollision.IsBackCorner();
    }

    private bool _isFrontCorner
    {
        get => _playerCollision.IsFrontCorner();
    }

    private int _side
    {
        get => transform.localScale.x > 0 ? 1 : -1;
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

        //Get the last checkpoint position from PlayerPrefs
        string checkpointData = PlayerPrefs.GetString("LastCheckpoint", "0;-2.63");
        string[] splitData = checkpointData.Split(';');

        float x = float.Parse(splitData[0], CultureInfo.InvariantCulture);
        float y = float.Parse(splitData[1], CultureInfo.InvariantCulture);

        Vector2 checkpointPosition = new Vector2(x, y);
        if (checkpointPosition.y != -2.63f) FindObjectOfType<TutorialTrigger>().CloseTutorial();
        transform.position = checkpointPosition;
        foreach (Checkpoint checkpoint in FindObjectsOfType<Checkpoint>())
        {
            if ((int)checkpoint.gameObject.transform.position.y == (int)checkpointPosition.y)
            {
                checkpoint.ActivateCheckpoint();
                break;
            }
        }
    }

    private void Update()
    {

        if (!_isGrounded)
        {
            if (_isBackCorner && _isFrontCorner)
            {
                return;
            }
        }

        CheckButtonPressed();
    }

    void FixedUpdate()
    {
        //If player falls for x seconds, then it will be hurted for x seconds
        if (!_isGrounded && _rb.velocity.y < 0 && !_anim.GetBool("isHurted"))
        {
            _timeFalling += Time.deltaTime;
            if (_timeFalling >= _timeToFall)
            {
                _anim.SetBool("isHurted", true);
            }
        }

        if (_isGrounded)
        {
            _timeFalling = 0f;
            if (_anim.GetBool("isHurted"))
            {
                if (_timeHurt <= _timeToHurt)
                {
                    _timeHurt += Time.deltaTime;
                    return;
                }
                else
                {
                    _anim.SetBool("isHurted", false);
                    _anim.SetBool("hitWall", false);
                    _timeHurt = 0f;
                }
            }
        }

        if (_rb.velocity.y > 0 && !_isGrounded)
        {
            _rb.sharedMaterial = _bounceMaterial;
        }
        else
        {
            _rb.sharedMaterial = null;
        }

        //For when the player touches the ground
        if (_isGrounded && _jumpValue == 0)
        {
            _rb.velocity = new Vector2(0, _rb.velocity.y);
            _anim.SetBool("hitWall", false);
        }


        // Handle wall collision and bouncing
        if (_rb.velocity.y != 0)
        {
            if (_hasWallInFront)
            {
                _anim.SetBool("hitWall", true);
                _rb.velocity = new Vector2(-_bounceForce * 
                    _rb.velocity.x, _rb.velocity.y);

                Flip();
            }

        }

        //For when the player jumps at max force
        if (_jumpValue >= _maxJumpValue && _isGrounded)
        {
            float tempx = this.transform.localScale.x * _lateralForce;
            float tempy = _jumpValue;
            if (_leftButton.buttonPressed && _rightButton.buttonPressed)
            {
                tempx = 0;
            }
            _rb.velocity = new Vector2(tempx, tempy);
            _anim.SetBool("isPreJumping", false);
            SetLastJumpForce(_jumpValue);
            _anim.SetTrigger("Jump");
            Invoke("ResetJump", 0.2f);
        }

        //For when the player jumps
        if (_isJumping)
        {
            if (_isGrounded)
            {
                if (_jumpValue != 0 && _jumpValue < _minJumpValue) _jumpValue = _minJumpValue;
                _rb.velocity = new Vector2(this.transform.localScale.x * _lateralForce, _jumpValue);
                SetLastJumpForce(_jumpValue);
                _anim.SetTrigger("Jump");
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
            Flip(-1);
            _jumpValue += 0.42f;
            _anim.SetBool("isPreJumping", true);
            return;
        }
        else if (_rightButton.buttonPressed && _isGrounded)
        {
            Flip(1);
            _jumpValue += 0.42f;
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

    private void Flip()
    {
        transform.localScale = new Vector3(-1 * transform.localScale.x, 1, 1);
    }

    private void Flip(int x)
    {
        transform.localScale = new Vector3(x, 1, 1);
    }

}
