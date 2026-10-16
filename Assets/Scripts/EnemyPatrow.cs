using System.Collections; // Necesario para IEnumerator
using UnityEngine;

public class EnemyPatrow : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float speed = 2f;
    public bool canMove = true; // <-- Controla si puede moverse o no
    private bool movingRight = false;

    [Header("Detector")]
    public Transform groundCheck;
    public float distanceToGround = 1.5f;
    public float distanceToWall = 1.5f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();    
    }

    void Update()
    {
        // Si NO puede moverse (porque está herido o muerto), frenamos en X y no hacemos nada más
        if (!canMove)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        rb.linearVelocity = new Vector2(movingRight ? speed : -speed, rb.linearVelocity.y);
        Vector2 direction = movingRight ? Vector2.right : Vector2.left;

        RaycastHit2D groundAhead = Physics2D.Raycast(groundCheck.position, Vector2.down, distanceToGround, groundLayer);
        RaycastHit2D wallAhead = Physics2D.Raycast(transform.position, direction, distanceToWall, groundLayer);

        if (groundAhead.collider == null || wallAhead.collider != null)
        {
            Flip();
        }
    }

    // Función para congelar al enemigo durante el golpe
    public void FreezeMovement(float duration)
    {
        StartCoroutine(FreezeRoutine(duration));
    }

    private IEnumerator FreezeRoutine(float duration)
    {
        canMove = false;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Frena en seco
        yield return new WaitForSeconds(duration);   // Espera a que pase la animación de daño
        canMove = true;                              // Vuelve a patrullar
    }

    void Flip()
    {
        movingRight = !movingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDrawGizmos()
    {
        if(groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(groundCheck.position, Vector2.down * distanceToGround);
        }
        Vector2 direction = movingRight ? Vector2.right : Vector2.left;
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, direction * distanceToWall);
    }
}