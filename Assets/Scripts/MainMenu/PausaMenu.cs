using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PausaMenu : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject panelPausa;

    [Header("Estado Pausa")]
    public bool juegoPausado = false;

    void Start()
    {
        // Al iniciar la partida, ocultamos el menú y aseguramos que el juego no esté pausado
        Reanudar();
    }

    void Update()
    {
        bool pressEscape = Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame;
        bool pressP = Keyboard.current != null && Keyboard.current.pKey.wasPressedThisFrame;

        if (pressEscape || pressP)
        {
            if (juegoPausado)
            {
                Reanudar();
            }
            else
            {
                Pausar();
            }
        }
    }

    public void Reanudar()
    {
        if (panelPausa != null) panelPausa.SetActive(false);
        Time.timeScale = 1f;
        juegoPausado = false;
    }

    public void Pausar()
    {
        if (panelPausa != null) panelPausa.SetActive(true);
        Time.timeScale = 0f;
        juegoPausado = true;
    }

    public void Reiniciar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void IrAMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}