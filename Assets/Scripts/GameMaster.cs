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
    public GameObject pausePanel;
    public TrailCollider trailCollider;
    public FollowTouch followTouch;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update() 
    {
        CheckInputs();
    }

    void CheckInputs()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName == "MainMenu" & Input.GetKeyDown(KeyCode.Return))
        {
            GoToGameScene();
        }
        if (sceneName == "Game" & (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)))
        {
            backgroundMusic = GameObject.FindGameObjectWithTag("Background Music").GetComponent<AudioSource>();

            // if (!isPaused)
            // {
            //     pausePanel.SetActive(true);
            //     backgroundMusic.Pause();
            //     audioSource.PlayOneShot(pause, 0.75f);
            //     Time.timeScale = 0;
            // }
            // else
            // {
            //     pausePanel.SetActive(false);
            //     Time.timeScale = 1;
            //     backgroundMusic.UnPause();
            // }
            // isPaused = !isPaused;
            // trailCollider.isPaused = !trailCollider.isPaused;
            // followTouch.isPaused = !followTouch.isPaused;
        }
    }

    public void UnPause() {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        backgroundMusic.UnPause();
        isPaused = !isPaused;
        trailCollider.isPaused = !trailCollider.isPaused;
        // followTouch.isPaused = !followTouch.isPaused;
    }

    //Use for physical pause button

    // public void Pause(){
    //     pausePanel = GameObject.FindGameObjectWithTag("Pause Menu");
    //     if (SceneManager.GetActiveScene().name == "Game"){
    //     pausePanel.SetActive(true);
    //     backgroundMusic.Pause();
    //     audioSource.PlayOneShot(pause, 0.75f);
    //     Time.timeScale = 0;
    //     isPaused = !isPaused;
    //     }
    // }

    public void GoToGameScene() {
        SceneManager.LoadScene("Game");
        Time.timeScale = 1;
        isPaused = false;
        trailCollider.isPaused = !trailCollider.isPaused;
        // followTouch.isPaused = !followTouch.isPaused;
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
        isPaused = false;
        trailCollider.isPaused = !trailCollider.isPaused;
        // followTouch.isPaused = !followTouch.isPaused;
    }

    public void GoToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame() {
        Application.Quit();
    }

}
