using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private Vector2 _boxSize;
    [SerializeField] private Vector2 _boxOffset;
    [SerializeField] private LayerMask _groundMask;
    private Animator _anim;

    private void Awake() 
    {
        _anim = GetComponentInChildren<Animator>();
    }
    public bool HasWallInFront()
    {
        return Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x
        + (_boxOffset.x * transform.localScale.x), gameObject.transform
        .position.y + _boxOffset.y), _boxSize, 0f, _groundMask);
    }

    public bool IsGrounded()
    {
        bool isGrounded = Physics2D.OverlapBox(new Vector2(transform.position.x, 
            transform.position.y - 0.5f), 
            new Vector2(0.5f, 0.1f), 0f, _groundMask);

        _anim.SetBool("isGrounded", isGrounded);
        return isGrounded;
    }

// #if UNITY_EDITOR
//     private void OnDrawGizmosSelected()
//     {
//         Gizmos.color = Color.green;
//         Gizmos.DrawWireCube(new Vector2(gameObject.transform.position.x
//         + (_boxOffset.x * transform.localScale.x), gameObject.transform
//         .position.y + _boxOffset.y), _boxSize);
//     }
// #endif
}
