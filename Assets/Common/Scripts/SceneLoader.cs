using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadMenuMain();
        }
    }

    // This is a tiny waiting time so that we can hear the click of the buttons
    [SerializeField] float waitBeforeLoadSeconds = 0.1f;
    private IEnumerator WaitBeforeLoad(string sceneToLoad)
    {
        FindObjectOfType<MemoryBetweenScenes>().SetLastSceneIndex(SceneManager.GetActiveScene().buildIndex);
        yield return new WaitForSeconds(waitBeforeLoadSeconds);
        SceneManager.LoadScene(sceneToLoad);
    }

    // Main
    public void LoadMenuMain()
    {
        Time.timeScale = 1;
        StartCoroutine(WaitBeforeLoad("Menu - Main"));
    }

    public void ReloadScene()
    {
        Time.timeScale = 1;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void LoadLastScene()
    {
        Time.timeScale = 1;
        int lastSceneIndex = FindObjectOfType<MemoryBetweenScenes>().GetLastSceneIndex();
        SceneManager.LoadScene(lastSceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Pixel Pirate
    public void LoadPPMenuStart()
    {
        Time.timeScale = 1;
        StartCoroutine(WaitBeforeLoad("PP - Menu - Start"));
    }

    public void LoadPPMenuEnd()
    {
        Time.timeScale = 1;
        StartCoroutine(WaitBeforeLoad("PP - Menu - End"));
    }

    public void LoadPPGameMain()
    {
        Time.timeScale = 1;
        StartCoroutine(WaitBeforeLoad("PP - Game - Main"));
    }

    // Canyon Racing
    public void LoadCRMenuStart()
    {
        Time.timeScale = 1;
        StartCoroutine(WaitBeforeLoad("CR - Menu - Start"));
    }

    public void LoadCRMenuEnd()
    {
        Time.timeScale = 1;
        StartCoroutine(WaitBeforeLoad("CR - Menu - End"));
    }

    public void LoadCRLevel(int level)
    {
        Time.timeScale = 1;
        StartCoroutine(WaitBeforeLoad("CR - Level " + level.ToString()));
    }

    // Bib Town
    public void LoadBTSandbox()
    {
        Time.timeScale = 1;
        StartCoroutine(WaitBeforeLoad("BT - Sandbox"));
    }
}