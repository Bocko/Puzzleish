using UnityEngine;

public class MenuUI : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.instance.LoadScene(SceneIndexes.TUTORIALISH);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
