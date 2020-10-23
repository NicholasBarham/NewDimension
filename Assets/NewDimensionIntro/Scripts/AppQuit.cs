using UnityEngine;
using UnityEngine.SceneManagement;

public class AppQuit : MonoBehaviour
{
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
         Application.OpenURL(webplayerQuitURL);
#else
         Application.Quit();
#endif
    }

    public void PlayIntro()
    {
        SceneManager.LoadScene(0);
    }

    public void PlayMainGame()
    {
        SceneManager.LoadScene(1);
    }

    public void PlayEndGame()
    {
        SceneManager.LoadScene(2);
    }
}
