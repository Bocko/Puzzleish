using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public GameObject pauseScreen;
    public GameObject buttonHolder;
    public GameObject helpScreen;
    bool isPaused = false;
    bool helpOpen = false;
    PlayerLook playerLook;

    void Start()
    {
        playerLook = GameObject.Find("First Person Player").GetComponent<PlayerLook>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                if (helpOpen)
                {
                    HideHelp();
                    return;
                }
                ResumeGame();
                isPaused = false;
            }
            else
            {
                PauseGame();
                isPaused = true;
            }
        }
    }

    private void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        playerLook.enabled = false;
        pauseScreen.SetActive(true);
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
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
        buttonHolder.SetActive(false);
        helpScreen.SetActive(true);
        helpOpen = true;
    }

    public void HideHelp()
    {
        buttonHolder.SetActive(true);
        helpScreen.SetActive(false);
        helpOpen = false;
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
