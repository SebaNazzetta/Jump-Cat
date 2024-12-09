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
    [SerializeField] private Vector2 _cornerBoxSize;
    [SerializeField] private Vector2 _cornerBoxOffset;
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

    public bool IsCorner()
    {
        bool isCorner = Physics2D.OverlapBox(new Vector2((gameObject.transform.position.x
         + (_cornerBoxOffset.x * transform.localScale.x) * -1),
            gameObject.transform.position.y - _cornerBoxOffset.y), _cornerBoxSize, 0f, _groundMask);
        return isCorner;
    }

 #if UNITY_EDITOR
     private void OnDrawGizmosSelected()
     {
        // Draw a box for walls
         Gizmos.color = Color.green;
         Gizmos.DrawWireCube(new Vector2(gameObject.transform.position.x
         + (_wallBoxOffset.x * transform.localScale.x), gameObject.transform
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

        // Draw two box for ground corners
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector2((gameObject.transform.position.x
         + (_cornerBoxOffset.x * transform.localScale.x) * -1),
            gameObject.transform.position.y - _cornerBoxOffset.y), _cornerBoxSize);
    }

 #endif
}
