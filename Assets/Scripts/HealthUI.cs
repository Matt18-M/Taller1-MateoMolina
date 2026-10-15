
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [Tooltip("Arrastra aquí el Player que contiene el script PlayerHealth.")]
    public PlayerHealth playerHealth;

    [Header("Corazones")]
    [Tooltip("Arrastra aquí las 3 imágenes de corazones del Canvas.")]
    public Image[] hearts;

    // Guarda la cantidad de vidas mostrada anteriormente
    private int previousHealth = -1;

    void Start()
    {
        // Comprobar que el jugador esté asignado
        if (playerHealth == null)
        {
            Debug.LogError("Debes asignar Player Health en el Inspector.");
            return;
        }

        // Actualizar los corazones al iniciar
        UpdateHearts();
    }

    void Update()
    {
        // Actualizar la interfaz solo cuando cambien las vidas
        if (playerHealth != null &&
            playerHealth.CurrentHealth != previousHealth)
        {
            UpdateHearts();
        }
    }

    void UpdateHearts()
    {
        // Obtener las vidas actuales del jugador
        int health = playerHealth.CurrentHealth;

        // Mostrar u ocultar cada corazón según las vidas
        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] != null)
            {
                hearts[i].enabled = i < health;
            }
        }

        // Guardar el último valor mostrado
        previousHealth = health;
    }
}
