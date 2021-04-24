using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterDeath : MonoBehaviour
{
    [SerializeField] private GameObject deathScreenPrefab;
    // Start is called before the first frame update

    private Transform mainCanvas;

    void Start()
    {
        mainCanvas = GameObject.Find("HUD").transform;
    }
    public void Trigger()
    {
        Time.timeScale = 0f;
        GetComponent<CharacterRotation>().enabled = false;
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        var deathScreen = GameObject.Instantiate(deathScreenPrefab, mainCanvas);
        yield return new WaitForSecondsRealtime(2f);
        SceneManager.LoadScene("TitleScene");
    }
}
