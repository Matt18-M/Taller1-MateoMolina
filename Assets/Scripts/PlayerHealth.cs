using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vidas")]
    public int maxHealth = 3;
    private int currentHealth;
    public int CurrentHealth => currentHealth;

    [Header("Daño y rechazo")]
    public float invulnerabilityTime = 1f;
    public float knockbackForce = 6f;
    public float knockbackDuration = 0.2f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isInvulnerable;
    private bool isKnockedBack;

    public bool IsKnockedBack => isKnockedBack;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    // Detección física directa contra enemigos
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            RecibirDanio(collision.transform, 1);
        }
    }

    // Gestión del daño recibido: descuenta vida, activa feedback visual, animación e inmunidad
    public void RecibirDanio(Transform atacante, int cantidad = 1)
    {
        if (isInvulnerable || currentHealth <= 0)
            return;

        currentHealth -= cantidad;

        // Feedback visual inmediato en color rojo
        if (spriteRenderer != null)
        {
            StartCoroutine(FlashRed());
        }

        // Activación de la animación de dolor mediante el Trigger Hurt original
        if (animator != null)
        {
            animator.ResetTrigger("Hurt");
            animator.SetTrigger("Hurt");
        }

        // Reinicio de nivel al quedarse sin vidas
        if (currentHealth <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        StartCoroutine(InvulnerabilityCoroutine());

        // Dirección del rechazo: empuja hacia los lados para no lanzarlo tan alto
        Vector2 direction = (transform.position - atacante.position).normalized;

        if (Mathf.Abs(direction.x) < 0.1f)
        {
            direction.x = (transform.position.x >= atacante.position.x) ? 1f : -1f;
        }

        StartCoroutine(KnockbackCoroutine(direction));
    }

    // Parpadeo rojo visual que confirma el daño en pantalla
    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        spriteRenderer.color = Color.white;
    }

    // Tiempo de inmunidad tras recibir un golpe
    IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityTime);
        isInvulnerable = false;
    }

    // Aplica el empuje y bloquea el movimiento temporalmente
    IEnumerator KnockbackCoroutine(Vector2 direction)
    {
        isKnockedBack = true;
        rb.linearVelocity = Vector2.zero;

        // Empuje equilibrado (más horizontal y leve rebote vertical)
        Vector2 force = new Vector2(direction.x * 0.8f, 0.4f).normalized * knockbackForce;
        rb.AddForce(force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackDuration);
        isKnockedBack = false;
    }
}