using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 3;
    private int currentHealth;

    private Animator animator;
    private EnemyPatrow enemyPatrow;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    // Inicializa la vida y busca referencias en el objeto, sus padres o sus hijos
    void Start()
    {
        currentHealth = maxHealth;

        animator = GetComponentInChildren<Animator>();
        if (animator == null) animator = GetComponentInParent<Animator>();

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer == null) spriteRenderer = GetComponentInParent<SpriteRenderer>();

        enemyPatrow = GetComponent<EnemyPatrow>();
        if (enemyPatrow == null) enemyPatrow = GetComponentInParent<EnemyPatrow>();
        if (enemyPatrow == null) enemyPatrow = GetComponentInChildren<EnemyPatrow>();

        rb = GetComponent<Rigidbody2D>();
        if (rb == null) rb = GetComponentInParent<Rigidbody2D>();
    }

    // Procesa el impacto: resta vida, da feedback visual, frena al enemigo e inicia la animación de daño
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Feedback visual inmediato
        if (spriteRenderer != null)
        {
            StartCoroutine(FlashRed());
        }

        // Frena la inercia y pausa la patrulla para que no camine mientras sufre el golpe
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
        if (enemyPatrow != null)
        {
            enemyPatrow.FreezeMovement(0.35f);
        }

        // Fuerza la animación de dolor de inmediato sin esperar transiciones del Animator
        if (animator != null)
        {
            animator.Play("hurtEnemy", 0, 0f);
        }

        // Comprobación de muerte
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Hace que el enemigo parpadee en color rojo al recibir un impacto
    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        spriteRenderer.color = Color.white;
    }

    // Desactiva el movimiento y elimina al enemigo de la escena
    void Die()
    {
        if (enemyPatrow != null)
        {
            enemyPatrow.canMove = false;
        }

        Destroy(gameObject);
    }
}