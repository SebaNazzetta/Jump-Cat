using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Globalization;

public class PlayerController : MonoBehaviour
{
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

    [Header("VFX")]
    [SerializeField] private GameObject _jumpVFX;
    [SerializeField] private GameObject _bigJumpVFX;
    [SerializeField] private Transform _jumpVFXPosition;
    [SerializeField] private Transform _bigJumpVFXPosition;
    public GameObject particleOnDeath;
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

    private bool _isBackCorner
    {
        get => _playerCollision.IsBackCorner();
    }

    private bool _isFrontCorner
    {
        get => _playerCollision.IsFrontCorner();
    }
    private bool _canJump
    {
        get => _spriteRenderer.enabled && !_anim.GetCurrentAnimatorStateInfo(0)
            .IsName("Player_Hurt") && !_anim.GetBool("isHurted"); 
    }
    
    private Rigidbody2D _rb;
    private bool _isJumping = false;
    private Animator _anim;
    private PlayerCollision _playerCollision;
    private bool _playedVFXOnce;
    private bool _waitingTilGrounded;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _checkpointPosition;
    private bool _releasedJump = false;
    private bool _jumpedWithThisButton = false;
    [SerializeField] private Animator _catIcon;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();
        _spriteRenderer = _anim.GetComponent<SpriteRenderer>();
        _playerCollision = GetComponent<PlayerCollision>();

        //Get the last checkpoint position from PlayerPrefs
        string checkpointData = PlayerPrefs.GetString("LastCheckpoint", "0;-2.67");
        string[] splitData = checkpointData.Split(';');
        float x = float.Parse(splitData[0], CultureInfo.InvariantCulture);
        float y = float.Parse(splitData[1], CultureInfo.InvariantCulture);
        _checkpointPosition = new Vector2(x, y);
        if (_checkpointPosition.y != -2.63f) FindObjectOfType<TutorialTrigger>().CloseTutorial();
        transform.position = _checkpointPosition;

    }
    private void Start()
    {
        foreach (Checkpoint checkpoint in FindObjectsOfType<Checkpoint>())
        {
            if ((int)checkpoint.gameObject.transform.position.y == (int)_checkpointPosition.y)
            {
                FindObjectOfType<CheckpointManager>().lastCheckpointPosition = transform.position;
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

            //For when the player stands in a corner in the back
            if (_isBackCorner && !_isGrounded)
            {
                _rb.velocity = new Vector2((_bounceForce + 0.7f) * (transform.localScale.x),
                    _rb.velocity.y + 0.3f);
            }

            //For when the player stands in a corner in the back
            if (_isFrontCorner && !_isGrounded)
            {
                transform.localScale = new Vector3(-1 * transform.localScale.x, 1, 1);
                _rb.velocity = new Vector2((_bounceForce + 0.7f) * (transform.localScale.x),
                    _rb.velocity.y + 0.3f);
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
                if(_canJump)
                {
                    _anim.SetBool("isHurted", true);
                    _catIcon.SetTrigger("Hurt");
                    _playedVFXOnce = false;
                }
                else
                {
                    _playedVFXOnce = true;
                    _anim.SetBool("isHurted", false);
                    _anim.SetBool("hitWall", false);
                    _timeHurt = 0f;
                }
            }
            if(!_waitingTilGrounded)
            {
                StartCoroutine(WaitTilGrounded());
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
                    if (!_playedVFXOnce)
                    {
                        _playedVFXOnce = true;
                        StartCoroutine(InstantiateJumpVFX(true));
                    }
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
            var xVelocity = _playerCollision.isOnIce ? _rb.velocity.x : 0;
            _rb.velocity = new Vector2(xVelocity, _rb.velocity.y);
            _anim.SetBool("hitWall", false);
        }

        //For when the player touches a wall 
        if (_rb.velocity.y != 0 && _hasWallInFront)
        {
            _anim.SetBool("hitWall", true);
            _rb.velocity = new Vector2(-_bounceForce * _rb.velocity.x,
                _rb.velocity.y);

            transform.localScale = new Vector3(-1 * transform.localScale.x, 1, 1);
        }

        //For when the player jumps at max force
        if (_jumpValue >= _maxJumpValue && _isGrounded)
        {
            float tempx = this.transform.localScale.x * _lateralForce;
            float tempy = _jumpValue;

            if ((_leftButton.buttonPressed && _rightButton.buttonPressed) || (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow)))
            {
                tempx = 0;
            }

            _rb.velocity = new Vector2(tempx, tempy);
            _anim.SetBool("isPreJumping", false);
            SetLastJumpForce(_jumpValue);
            _jumpedWithThisButton = true;
            _anim.SetTrigger("Jump");
            Invoke("ResetJump", 0.2f);
            StartCoroutine(InstantiateJumpVFX(true));
        }

        //For when the player jumps
        if (_isJumping && _canJump)
        {
            if (_isGrounded)
            {
                if (_jumpValue != 0 && _jumpValue < _minJumpValue) _jumpValue = _minJumpValue;
                _rb.velocity = new Vector2(this.transform.localScale.x * _lateralForce, _jumpValue);
                SetLastJumpForce(_jumpValue);
                _jumpedWithThisButton = true;
                _anim.SetTrigger("Jump");
                _jumpValue = 0.0f;
                _isJumping = false;
                _anim.SetBool("isPreJumping", false);
                StartCoroutine(InstantiateJumpVFX());
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
        if (!_canJump)
        {
            _releasedJump = false;
            _jumpedWithThisButton = true;
            return;
        }
        if (!_jumpedWithThisButton)
        {
            if ((_leftButton.buttonPressed || Input.GetKey(KeyCode.LeftArrow)) && _isGrounded)
            {
                _releasedJump = false;
                transform.localScale = new Vector3(-1, 1, 1);
                _jumpValue += 0.42f;
                _anim.SetBool("isPreJumping", true);
                return;
            }
            else if ((_rightButton.buttonPressed || Input.GetKey(KeyCode.RightArrow)) && _isGrounded)
            {
                _releasedJump = false;
                transform.localScale = new Vector3(1, 1, 1);
                _jumpValue += 0.42f;
                _anim.SetBool("isPreJumping", true);
                return;
            }
        }

        if (!_leftButton.buttonPressed && !_rightButton.buttonPressed && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            _releasedJump = true;
            _jumpedWithThisButton = false;
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

    private IEnumerator InstantiateJumpVFX(bool big = false)
    {
        if(!_canJump)yield break;

        GameObject vfx = big ? _bigJumpVFX : _jumpVFX;
        Transform vfxPosition = big ? _bigJumpVFXPosition : _jumpVFXPosition;

        GameObject jumpVFX = Instantiate(vfx, vfxPosition.position,
            Quaternion.identity);

        Animator jumpVFXAnim = jumpVFX.GetComponent<Animator>();

        yield return new WaitUntil(() => jumpVFXAnim
            .GetCurrentAnimatorStateInfo(0).normalizedTime > 1 ||
            !_canJump);

        Destroy(jumpVFX);
    }

    private IEnumerator WaitTilGrounded()
    {
        _waitingTilGrounded = true;
        yield return new WaitUntil(() => _isGrounded);
        StartCoroutine(InstantiateJumpVFX());
        _waitingTilGrounded = false;
    }

    
}
