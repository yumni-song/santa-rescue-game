using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialNavigator : MonoBehaviour
{
    public string currentScene;   // Tutorial_1, Tutorial_2, Tutorial_3, Tutorial_4
    public bool isLeftButton;     // true = 왼쪽, false = 오른쪽

    public void OnClick()
    {
        // 예외 처리
        if (currentScene == "Tutorial_1" && isLeftButton)
        {
            SceneManager.LoadScene("StartScene");
            return;
        }

        if (currentScene == "Tutorial_4" && !isLeftButton)
        {
            SceneManager.LoadScene("InGameScene2");
            return;
        }

        // 기본 이동 처리
        int index = int.Parse(currentScene.Split('_')[1]);  // Tutorial_1 → 1

        if (isLeftButton)
        {
            index -= 1; // 이전 페이지
        }
        else
        {
            index += 1; // 다음 페이지
        }

        string nextScene = "Tutorial_" + index;
        SceneManager.LoadScene(nextScene);
    }
}

