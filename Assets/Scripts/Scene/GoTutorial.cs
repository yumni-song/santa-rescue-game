using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToTutorial : MonoBehaviour
{
    public void LoadTutorialScene()
    {
        SceneManager.LoadScene("Tutorial_1");
    }
}

