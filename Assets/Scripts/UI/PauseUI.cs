using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public enum PauseState { RUNNING, PAUSED, HELP }

    public GameObject pauseScreen;
    public GameObject buttonHolder;
    public GameObject helpScreen;

    public PlayerLook playerLook;

    PauseState pauseState = PauseState.RUNNING;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (pauseState)
            {
                case PauseState.RUNNING:
                    PauseGame();
                    break;
                case PauseState.PAUSED:
                    ResumeGame();
                    break;
                case PauseState.HELP:
                    HideHelp();
                    break;
            }
        }
    }

    private void PauseGame()
    {
        pauseState = PauseState.PAUSED;
        Cursor.lockState = CursorLockMode.None;

        playerLook.enabled = false;
        pauseScreen.SetActive(true);
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        pauseState = PauseState.RUNNING;
        Cursor.lockState = CursorLockMode.Locked;

        playerLook.enabled = true;
        pauseScreen.SetActive(false);
        Time.timeScale = 1;
    }

    public void Resume()
    {
        ResumeGame();
    }

    public void RestartLevel()
    {
        if (GameManager.instance != null)
        {
            ResumeGame();
            GameManager.instance.ReloadCurrentScene();
        }
    }

    public void ShowHelp()
    {
        pauseState = PauseState.HELP;

        buttonHolder.SetActive(false);
        helpScreen.SetActive(true);
    }

    public void HideHelp()
    {
        pauseState = PauseState.PAUSED;

        buttonHolder.SetActive(true);
        helpScreen.SetActive(false);
    }

    public void ExitToMenu()
    {
        if (GameManager.instance != null)
        {
            ResumeGame();
            Cursor.lockState = CursorLockMode.None;
            GameManager.instance.LoadScene(SceneIndexes.MENU_SCREEN);
        }
    }
}
