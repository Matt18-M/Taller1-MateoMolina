using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public void Jugar()
    {
        SceneManager.LoadScene("Nivel-1");
    }

    public void Salir()
    {
        Application.Quit();
    }

}
