using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void ToScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
