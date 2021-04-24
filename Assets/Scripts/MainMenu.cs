using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button exitButton;
    // Start is called before the first frame update

    private void PlayGame()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    private void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }
    void Start()
    {
        Time.timeScale = 1f;
        playButton.onClick.AddListener(PlayGame);

        #if UNITY_WEBGL
            exitButton.SetActive(false);
        #else
            exitButton.onClick.AddListener(ExitGame);
        #endif
    }
}
