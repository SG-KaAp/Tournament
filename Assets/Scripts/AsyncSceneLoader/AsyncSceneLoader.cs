using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AsyncSceneLoader : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Image loadingProgressBar;
    private AsyncOperation _level;
    public void LoadScene(string SceneName)
    {
        _level = SceneManager.LoadSceneAsync(SceneName);
        menuPanel.SetActive(false);
        loadingPanel.SetActive(true);
        _level.allowSceneActivation = false;
        while (_level.progress < 0.9f)
        {
            loadingProgressBar.fillAmount = _level.progress;
        }
        _level.allowSceneActivation = true;
    }
}