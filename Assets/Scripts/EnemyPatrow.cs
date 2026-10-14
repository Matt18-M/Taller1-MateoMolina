using UnityEngine;

public class EnemyPatrow : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float speed = 2f;
    private bool movingRight = false ;


    [Header("Detector")]
    public Transform groundCheck;
    public float distanceToGround = 1.5f;
    public float distanceToWall = 1.5f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();    
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(movingRight ? speed : -speed, rb.velocity.y);
        Vector2 direction = movingRight ? Vector2.right : Vector2.left;

        RaycastHit2D groundAhead = Physics2D.Raycast(groundCheck.position, Vector2.down, distanceToGround, groundLayer);
        RaycastHit2D wallAhead = Physics2D.Raycast(transform.position, direction, distanceToWall, groundLayer);

        if (groundAhead.collider == null || wallAhead.collider != null)
        {
            Flip();
        }
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
