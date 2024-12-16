using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private Vector2 _groundBoxSize;
    [SerializeField] private Vector2 _groundBoxOffset;
    [SerializeField] private Vector2 _headBoxSize;
    [SerializeField] private Vector2 _headBoxOffset;
    [SerializeField] private Vector2 _wallBoxSize;
    [SerializeField] private Vector2 _wallBoxOffset;
    [SerializeField] private Vector2 _backCornerBoxSize;
    [SerializeField] private Vector2 _backCornerBoxOffset;
    [SerializeField] private Vector2 _frontCornerBoxSize;
    [SerializeField] private Vector2 _frontCornerBoxOffset;
    [SerializeField] private LayerMask _groundMask;
    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
    }

    public bool HasWallInFront()
    {
        return Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x
        + (_wallBoxOffset.x * transform.localScale.x), gameObject.transform
        .position.y + _wallBoxOffset.y), _wallBoxSize, 0f, _groundMask);
    }

    public bool HasWallBehind()
    {
        return Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x
        + (_wallBoxOffset.x * transform.localScale.x) * -1, gameObject.transform
        .position.y + _wallBoxOffset.y), _wallBoxSize, 0f, _groundMask);
    }

    public bool IsRoof()
    {
        return Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x
                   + (_headBoxSize.x * transform.localScale.x), gameObject.transform
                          .position.y + _headBoxOffset.y), _wallBoxSize, 0f, _groundMask);
    }

    public bool IsGrounded()
    {
        bool isGrounded = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x,
            gameObject.transform.position.y - _groundBoxOffset.y), _groundBoxSize, 0f, _groundMask);

        _anim.SetBool("isGrounded", isGrounded);
        return isGrounded;
    }

    public bool IsBackCorner()
    {
        bool isBackCorner = Physics2D.OverlapBox(new Vector2((gameObject.transform.position.x
         + (_backCornerBoxOffset.x * transform.localScale.x) * -1),
            gameObject.transform.position.y - _backCornerBoxOffset.y), _backCornerBoxSize, 0f, _groundMask);
        return isBackCorner;
    }

    public bool IsFrontCorner()
    {
        bool isFrontCorner = Physics2D.OverlapBox(new Vector2((gameObject.transform.position.x
         + (_frontCornerBoxOffset.x * transform.localScale.x) * -1),
            gameObject.transform.position.y - _frontCornerBoxOffset.y), _frontCornerBoxSize, 0f, _groundMask);
        return isFrontCorner;
    }

#if UNITY_EDITOR
     private void OnDrawGizmosSelected()
     {
        // Draw a box for walls
         Gizmos.color = Color.green;
         Gizmos.DrawWireCube(new Vector2(gameObject.transform.position.x
         + (_wallBoxOffset.x * transform.localScale.x), gameObject.transform
         .position.y + _wallBoxOffset.y), _wallBoxSize);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector2(gameObject.transform.position.x
         + (_wallBoxOffset.x * transform.localScale.x) * -1, gameObject.transform
         .position.y + _wallBoxOffset.y), _wallBoxSize);

        // Draw a box for ground
        Gizmos.color = Color.green;
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x
         + (_groundBoxOffset.x * transform.localScale.x), 
            gameObject.transform.position.y - _groundBoxOffset.y), _groundBoxSize);

        // Draw a box for roof
        Gizmos.color = Color.green;
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x
                    + (_headBoxOffset.x * transform.localScale.x), gameObject.transform
                            .position.y + _headBoxOffset.y), _headBoxSize);

        // Draw a box for ground corner in back
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector2((gameObject.transform.position.x
         + (_backCornerBoxOffset.x * transform.localScale.x) * -1),
            gameObject.transform.position.y - _backCornerBoxOffset.y), _backCornerBoxSize);

        // Draw a box for ground corner in front
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector2((gameObject.transform.position.x
         + (_frontCornerBoxOffset.x * transform.localScale.x) * -1),
            gameObject.transform.position.y - _frontCornerBoxOffset.y), _frontCornerBoxSize);
    }

#endif
}
