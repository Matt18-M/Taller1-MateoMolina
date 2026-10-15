
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Contador estático que guarda la cantidad de monedas recolectadas.
    // Al ser static, se comparte entre todas las monedas de la escena.
    public static int CoinsCollected = 0;

    // Reinicia el contador de monedas al comenzar una nueva partida.
    public static void ResetCoins()
    {
        CoinsCollected = 0;
    }

    // Detecta cuando otro objeto entra en el área de la moneda.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Comprueba si el objeto que tocó la moneda es el jugador.
        if (other.CompareTag("Player"))
        {
            // Incrementa en uno la cantidad de monedas recolectadas.
            CoinsCollected++;

            // Muestra en la consola la cantidad actual de monedas.
            Debug.Log("Monedas: " + CoinsCollected);

            // Destruye la moneda que acaba de recoger el jugador.
            Destroy(gameObject);
        }
    }
}
