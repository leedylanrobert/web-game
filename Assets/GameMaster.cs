using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    private bool isPaused = false;
    private AudioSource backgroundMusic;
    private AudioSource audioSource;
    public AudioClip pause;

    void Start()
    {
        backgroundMusic = GameObject.FindGameObjectWithTag("Background Music").GetComponent<AudioSource>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName == "MainMenu" & Input.GetKeyDown(KeyCode.Return))
        {
            GoToGameScene();
        }
        if (sceneName == "Game" & (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)))
        {
            Debug.Log("Is Paused? " + isPaused);
            if (!isPaused)
            {
                backgroundMusic.Pause();
                audioSource.PlayOneShot(pause, 0.75f);
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
                backgroundMusic.UnPause();
            }
            isPaused = !isPaused;
        }
    }

    public void GoToGameScene() {
        SceneManager.LoadScene("Game");
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame() {
        Application.Quit();
    }

}
