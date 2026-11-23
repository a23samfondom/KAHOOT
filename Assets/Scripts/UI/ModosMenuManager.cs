using UnityEngine;
using UnityEngine.SceneManagement;

public class ModosMenuManager : MonoBehaviour
{
    public void ModoSeleccionar()
    {
        SceneManager.LoadScene("SelectorMenu");
    }

    public void ModoLeaderboards()
    {
        SceneManager.LoadScene("LeaderboardMenu");
    }

    public void ModoInformes()
    {
        SceneManager.LoadScene("InformesMenu");
    }
}
