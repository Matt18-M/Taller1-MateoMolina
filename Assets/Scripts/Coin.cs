using UnityEngine;

public class Coin : MonoBehaviour
{
    // Contador compartido que almacena el total de monedas recolectadas.
    public static int CoinsCollected = 0;

    // Reinicia el contador al comenzar una nueva partida.
    public static void ResetCoins()
    {
        CoinsCollected = 0;
    }

    // Detecta cuando el jugador recoge la moneda, reproduce el sonido,
    // actualiza el contador y elimina la moneda de la escena.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CoinsCollected++;

            Debug.Log("Monedas: " + CoinsCollected);

            // Reproduce el efecto de sonido al recoger la moneda.
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.ReproducirSfx(
                    AudioManager.Instance.sonidoMoneda
                );
            }

            Destroy(gameObject);
        }
    }
}
