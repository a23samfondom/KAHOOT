using UnityEngine;
using UnityEngine.SceneManagement;

public class ModosMenuManager : MonoBehaviour
{
    public void ModoSeleccionar()
    {
        SceneManager.LoadScene("SelectorMenu");
    }
}
