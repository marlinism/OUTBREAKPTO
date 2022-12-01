using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public static bool MapInUsed = false;
    public GameObject pauseMenuUI;
    public GameObject mapUI;
    

    // Update is called once per frame
    void Update()
    {
     if(Input.GetKeyDown(KeyCode.Escape)) {
         if(GameIsPaused) {
             Resume();
         } else {
             Pause();
         }
     }   

     if(Input.GetKeyDown(KeyCode.M)) {
         if(MapInUsed) {
             RemoveMap();
         } else {
             LoadMap();
         }
     }   
    }

    public void RemoveMap() {
        mapUI.SetActive(false);
        Time.timeScale = 1f;
        MapInUsed = false;
    }

    public void LoadMap() {
        mapUI.SetActive(true);
        Time.timeScale = 0f;
        MapInUsed = true;
    }

    public void Resume() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void LoadMenu() {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        Time.timeScale = 1f;
    }
}
