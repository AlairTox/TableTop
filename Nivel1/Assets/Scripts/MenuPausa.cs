
using System.Timers;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MenuPausa : MonoBehaviour
{
    [SerializeField] private GameObject menuPausa;
    [SerializeField] private GameObject menuSalir;
    
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

    public void MenuSalir(){
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Salir(){
        Debug.Log("/me Se cierra");
        Application.Quit();
    }
}
