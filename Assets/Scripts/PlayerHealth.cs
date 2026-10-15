
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    // Cantidad de vidas del jugador
    [Header("Vidas")]
    public int maxHealth = 3;
    private int currentHealth;
    public int CurrentHealth => currentHealth;

    // Configuración del daño y del empujón
    [Header("Daño y rechazo")]
    public float invulnerabilityTime = 1f;
    public float knockbackForce = 8f;
    public float knockbackDuration = 0.2f;

    // Componentes del jugador
    private Rigidbody2D rb;
    private Animator animator;

    // Controlan si el jugador es invulnerable
    // o está recibiendo el empujón
    private bool isInvulnerable;
    private bool isKnockedBack;

    // Permite que PlayerController consulte si hay empujón
    public bool IsKnockedBack => isKnockedBack;

    void Start()
    {
        // Obtener los componentes del mismo objeto Player
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Iniciar las vidas con el valor máximo
        currentHealth = maxHealth;
    }

    // Detectar la colisión física con un enemigo
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Ignorar la colisión si no es un enemigo,
        // si hay invulnerabilidad o si no quedan vidas
        if (!collision.gameObject.CompareTag("Enemy") ||
            isInvulnerable || currentHealth <= 0)
            return;

        // Restar una vida al recibir daño
        currentHealth--;

        // Activar la animación de daño mediante el Trigger "Hurt"
        if (animator != null)
            animator.ResetTrigger("Hurt"); // Reiniciar el Trigger para que se pueda activar nuevamente
            animator.SetTrigger("Hurt");

        // Si las vidas llegan a cero, reiniciar el nivel actual
        if (currentHealth <= 0)
        {
            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex
            );
            return;
        }

        // Activar un segundo de invulnerabilidad
        StartCoroutine(InvulnerabilityCoroutine());

        // Calcular la dirección contraria al enemigo
        Vector2 direction =
            (transform.position - collision.transform.position).normalized;

        // Si ambos objetos están alineados horizontalmente,
        // determinar el lado del empujón usando sus posiciones
        if (direction.x == 0f)
        {
            direction.x =
                transform.position.x >= collision.transform.position.x
                ? 1f : -1f;
        }

        // Aplicar el empujón al jugador
        StartCoroutine(KnockbackCoroutine(direction));
    }

    // Impedir que el jugador reciba daño durante un segundo
    IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;

        // Esperar el tiempo configurado en el Inspector
        yield return new WaitForSeconds(invulnerabilityTime);

        // Permitir recibir daño nuevamente
        isInvulnerable = false;
    }

    // Aplicar el rechazo y bloquear temporalmente el movimiento
    IEnumerator KnockbackCoroutine(Vector2 direction)
    {
        // Avisar al PlayerController que hay un empujón activo
        isKnockedBack = true;

        // Eliminar la velocidad anterior antes del rechazo
        rb.linearVelocity = Vector2.zero;

        // Aplicar fuerza horizontal y un poco de elevación
        rb.AddForce(
            new Vector2(direction.x, 0.6f).normalized * knockbackForce,
            ForceMode2D.Impulse
        );

        // Mantener el control bloqueado durante el empujón
        yield return new WaitForSeconds(knockbackDuration);

        // Devolver el control del movimiento al jugador
        isKnockedBack = false;
    }
}
