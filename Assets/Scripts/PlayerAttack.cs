using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Ajustes de Ataque")]
    public Transform attackPoint;
    public float attackRange = 1.8f;
    public LayerMask enemyLayer;
    public int attackDamage = 1;

    [Header("Sincronización y Tiempos")]
    [Tooltip("Tiempo en segundos que tarda la espada en conectar visualmente")]
    public float damageDelay = 0.15f; 
    [Tooltip("Tiempo de espera para poder dar el siguiente ataque")]
    public float attackCooldown = 0.5f; 
    private float nextAttackTime = 0f;

    private Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Comprueba el cooldown y detecta el botón de ataque (Z o Clic)
    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            bool pressZ = Keyboard.current != null && Keyboard.current.zKey.wasPressedThisFrame;
            bool clickMouse = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;

            if (pressZ || clickMouse)
            {
                StartAttack();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    // Inicia la animación del jugador y programa el daño para cuando la espada baje
    void StartAttack()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        Invoke(nameof(DealDamage), damageDelay);
    }

    // Detecta enemigos dentro del rango de impacto y les aplica el daño
    void DealDamage()
    {
        if (attackPoint == null) return;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponentInParent<EnemyHealth>();
            if (enemyHealth == null)
            {
                enemyHealth = enemy.GetComponent<EnemyHealth>();
            }

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
            }
        }
    }

    // Dibuja el círculo del rango de golpe en la ventana Scene para calibrarlo visualmente
    private void OnDrawGizmos()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}