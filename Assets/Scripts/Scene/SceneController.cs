using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // 재시작 버튼용
    public void LoadInGameScene()
    {
        SceneManager.LoadScene("InGameScene1");
    }

    // 메인화면 버튼용
    public void LoadStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }

    // 현재 씬 재시작 (보너스)
    public void RestartCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}