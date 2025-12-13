using UnityEngine;
using UnityEngine.SceneManagement;

public class WallSceneTrigger : MonoBehaviour
{
    public string nextSceneName = "InGameScene2";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}

