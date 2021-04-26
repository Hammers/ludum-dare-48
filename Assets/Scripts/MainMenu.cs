using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private string scene = "SampleScene";
    // Start is called before the first frame update

    private void PlayGame()
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    void Start()
    {
        Time.timeScale = 1f;
        playButton.onClick.AddListener(PlayGame);
    }
}
