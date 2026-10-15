using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rd;
    private Animator animator;
    private Keyboard keyboard;
    private PlayerHealth playerHealth;

    public float speed = 5f;
    public float jumpForce = 7f;

    private bool isGrounded;
    private bool facingRight = true;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        rd = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (rd == null)
        {
            Debug.LogError("Falta Rigidbody2D en el Player.");
        }

        if (animator == null)
        {
            Debug.LogError("Falta Animator en el Player.");
        }
    }

    void Update()
    {
        if (rd == null || animator == null)
            return;

        keyboard = Keyboard.current;

        if (keyboard == null)
            return;

        
        if (playerHealth != null && playerHealth.IsKnockedBack)
        {
            animator.SetFloat("Speed", 0f);
            return;
        }


        // Movimiento horizontal
        float move = 0f;

        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
        {
            move = -1f;
        }

        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
        {
            move = 1f;
        }

        // Animación de correr
        animator.SetFloat("Speed", Mathf.Abs(move));

        // Movimiento del personaje
        rd.linearVelocity = new Vector2(
            move * speed,
            rd.linearVelocity.y
        );

        // Voltear el personaje
        if (move > 0 && !facingRight)
        {
            Flip();
        }
        else if (move < 0 && facingRight)
        {
            Flip();
        }

        // Saltar
        if (keyboard.spaceKey.wasPressedThisFrame && isGrounded)
        {
            rd.linearVelocity = new Vector2(
                rd.linearVelocity.x,
                0f
            );

            rd.AddForce(
                Vector2.up * jumpForce,
                ForceMode2D.Impulse
            );

            isGrounded = false;
            animator.SetBool("isJump", true);
        }
    }

    // Detectar cuando toca el suelo
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;

            if (animator != null)
            {
                animator.SetBool("isJump", false);
            }
        }
    }

    // Detectar cuando deja de tocar el suelo
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;

            if (animator != null)
            {
                animator.SetBool("isJump", true);
            }
        }
    }

    // Voltear el personaje
    void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1f;

        transform.localScale = scale;
    }
}
