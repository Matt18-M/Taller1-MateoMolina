
using UnityEngine;
using TMPro;

public class CoinUI : MonoBehaviour
{
    [Header("Contador de monedas")]

    // Referencia al texto TextMeshPro que muestra las monedas en el Canvas.
    public TMP_Text coinText;

    // Se ejecuta una vez cuando comienza el juego.
    private void Start()
    {
        // Reinicia la cantidad de monedas al iniciar una nueva partida.
        Coin.ResetCoins();

        // Actualiza el texto para mostrar el contador inicial.
        UpdateCoinText();
    }

    // Se ejecuta una vez por cada fotograma.
    private void Update()
    {
        // Comprueba y actualiza el texto del contador.
        UpdateCoinText();
    }

    // Muestra en pantalla la cantidad actual de monedas recolectadas.
    private void UpdateCoinText()
    {
        // Comprueba que se haya asignado el texto en el Inspector.
        if (coinText != null)
        {
            // Actualiza el texto con la cantidad actual de monedas.
            coinText.text = "Monedas: " + Coin.CoinsCollected;
        }
    }
}
