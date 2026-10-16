using System.Collections;
using UnityEngine;

public class TimedSpikeTrap : MonoBehaviour
{
    [Header("Ajustes de Tiempo")]
    public float waitTime = 2f;
    public float activeTime = 1f;

    [Header("Ajustes de Daño")]
    public int damageAmount = 2;

    [Header("Estado Trampa")]
    public bool isDangerous = false;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(TrapRoutine());
    }

    IEnumerator TrapRoutine()
    {
        while (true)
        {
            isDangerous = false;
            yield return new WaitForSeconds(waitTime);

            if (animator != null)
            {
                animator.SetTrigger("Active");
            }

            isDangerous = true;
            yield return new WaitForSeconds(activeTime);

            isDangerous = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckAndDamage(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        CheckAndDamage(other);
    }

    private void CheckAndDamage(Collider2D other)
    {
        if (!isDangerous) return;

        PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();
        if (playerHealth == null)
        {
            playerHealth = other.GetComponent<PlayerHealth>();
        }

        if (playerHealth != null)
        {
            playerHealth.RecibirDanio(transform, damageAmount);
        }
    }
}