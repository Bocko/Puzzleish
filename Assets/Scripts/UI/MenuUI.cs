using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    public void StartGame()
    {
        print("start");
        GameManager.instance.LoadScene(SceneIndexes.TUTORIALISH);
    }
}
