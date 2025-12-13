using UnityEngine;
using UnityEngine.SceneManagement;

public class SledGoalTrigger : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string successSceneName = "HappyEnd";
    private bool triggered;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag(playerTag)) return;

        triggered = true;
        SceneManager.LoadScene(successSceneName);
    }
}
