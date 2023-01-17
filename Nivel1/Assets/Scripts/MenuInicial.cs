
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    private int color = 0;
    private string colorPrefsName = "color";

    private void Awake(){
        LoadData();
    }

    void Start(){

    }

    // Start is called before the first frame update
    public void Jugar(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Salir(){
        Application.Quit();
    }

    public void changeColor(){
        if(color == 0)
            color = 1;
        else if(color == 1)
            color = 0;
        Debug.Log("Color : " + color);
    }
    private void OnDestroy(){
        SaveData();
    }

    private void SaveData(){
        PlayerPrefs.SetInt(colorPrefsName, color);
    }

    private void LoadData(){
        color = PlayerPrefs.GetInt(colorPrefsName, 0);
    }
}
