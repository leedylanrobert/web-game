// using UnityEngine;
// using UnityEngine.EventSystems;
// using UnityEngine.UI;
 
// public class UI_Wall : MonoBehaviour, IPointerClickHandler
// {
// void OnMouseDown(){
//     Debug.Log("Sprite Clicked");
// }
//     private bool isPaused = false;
//     private AudioSource audioSource;
//     public AudioClip pause;
//     public GameObject pausePanel;
//     public void OnPointerClick(PointerEventData eventData)
//     {
//         if (!isPaused)
//             {
//                 pausePanel.SetActive(true);
//                 backgroundMusic.Pause();
//                 audioSource.PlayOneShot(pause, 0.75f);
//                 Time.timeScale = 0;
//             }
//             else
//             {
//                 pausePanel.SetActive(false);
//                 Time.timeScale = 1;
//                 backgroundMusic.UnPause();
//             }
//             isPaused = !isPaused;
//     }
 
// }