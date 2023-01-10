
using System.Timers;
using System;
using UnityEngine;
using UnityEngine.UI;


public class MenuPausa : MonoBehaviour
{
    [SerializeField] private GameObject menuPausa;
    Boolean gameIsPaused;
    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            gameIsPaused = !gameIsPaused;
            PauseGame();
        }
    }

    public void PauseGame (){
        if(gameIsPaused){
            Time.timeScale = 0f;
            menuPausa.SetActive(true);
        }else{
            Time.timeScale = 1f;
            menuPausa.SetActive(false);
        } 
    }
}
