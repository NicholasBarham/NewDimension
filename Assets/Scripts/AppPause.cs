using UnityEngine;
using UnityEngine.Events;

public class AppPause : MonoBehaviour
{
    [SerializeField]
    private PlayerController player = null;

    [SerializeField]
    private UnityEvent onPause = null;

    [SerializeField]
    private UnityEvent onResume = null;

    public static bool IsPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (player.enabled)
            {
                if (!player.IsDead)
                {
                    if (!IsPaused)
                        PauseGame();
                }
            }
            else
            {
                if (!player.IsDead)
                {
                    if (IsPaused)
                        ResumeGame();
                }
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        IsPaused = true;
        onPause?.Invoke();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        onResume?.Invoke();
    }
}
