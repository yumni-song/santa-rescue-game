using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void OnClickStart()
    {
        SceneManager.LoadScene("InGameScene1"); // 실제 게임 씬 이름 넣기
    }
}
