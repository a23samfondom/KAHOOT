using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public CanvasGroup hola;
    
    public void Jugar()
    {
        SceneManager.LoadScene("ModosMenu");
    }

    public void Options()
    {
        SceneManager.LoadScene("OptionMenu");
    }

    public void Creditos()
    {
        hola.alpha = 1;
    }
}
