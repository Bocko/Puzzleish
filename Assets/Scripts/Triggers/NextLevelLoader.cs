using UnityEngine;

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
