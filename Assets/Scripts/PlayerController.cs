using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float lateralForce = 6;
    public bool isGrounded;
    private Rigidbody2D rb;
    public LayerMask groundMask;
    public bool isJumping = false;

    public PhysicsMaterial2D bounceMat, normalMat;
    public float jumpValue = 0.0f;
    public float maxJumpValue;

    public ButtonPressed leftButton;
    public ButtonPressed rightButton;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CheckButtonPressed();
        isGrounded = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.5f),
        new Vector2(0.4f, 0.1f), 0f, groundMask);

        if (isGrounded && jumpValue == 0)
        {
            Debug.Log("toca el suelo");
            rb.velocity = new Vector2(0, 0);
        }

        if (jumpValue > 0)
        {
            rb.sharedMaterial = bounceMat;
        }
        else
        {
            rb.sharedMaterial = normalMat;
        }

        if(jumpValue >= maxJumpValue && isGrounded)
        {
            float tempx = this.transform.localScale.x * lateralForce;
            float tempy = jumpValue;
            rb.velocity = new Vector2(tempx, tempy);
            Invoke("ResetJump", 0.2f);
        }

        if(isJumping)
        {
            if(isGrounded)
            {
                rb.velocity = new Vector2(this.transform.localScale.x * lateralForce, jumpValue);
                jumpValue = 0.0f;
                isJumping = false;
            }
        }
    }

    void ResetJump()
    {
        jumpValue = 0;
    }

    public void CheckButtonPressed()
    {
        if(leftButton.buttonPressed && isGrounded)
        {
            this.gameObject.transform.localScale = new Vector3(-1, 1, 1);
            jumpValue += 0.1f;
            return;
        }
        else if(rightButton.buttonPressed && isGrounded)
        {
            this.gameObject.transform.localScale = new Vector3(1, 1, 1);
            jumpValue += 0.1f;
            return;
        }

        if(!leftButton.buttonPressed && !rightButton.buttonPressed)
        {
            if(jumpValue > 0)
            {
                isJumping = true;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.55f), new Vector2(0.4f, 0.1f));
    }
}
