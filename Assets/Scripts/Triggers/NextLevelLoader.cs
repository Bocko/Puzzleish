using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelLoader : MonoBehaviour
{
    public SceneIndexes nextLevel;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(GameManager.instance != null)
            {
                GameManager.instance.LoadScene(nextLevel);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
