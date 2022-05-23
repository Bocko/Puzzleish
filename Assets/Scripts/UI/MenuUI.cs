using UnityEngine;

public class MenuUI : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.instance.LoadScene(SceneIndexes.TUTORIALISH);
    }
}
