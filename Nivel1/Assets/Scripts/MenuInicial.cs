using System;
using System.Net.Mime;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuInicial : MonoBehaviour
{
    private int color = 0;
    private string colorPrefsName = "color";
    public Image oldImage;
    public Sprite black;
    public Sprite white;
    private void Awake(){
        LoadData();
    }

    void Start(){
        if(color == 0){oldImage.sprite = white;}
        if(color == 1){oldImage.sprite = black;}
    }
    // Start is called before the first frame update
    public void Jugar(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Salir(){
        Application.Quit();
    }

    public void changeColor(){
        if(color == 0){
            color = 1;
            oldImage.sprite = black;
        }
        else if(color == 1){
            color = 0;
            oldImage.sprite = white;
        }
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

    public void OpenPlotonium(){
        Application.OpenURL("https://noxne.itch.io/plotonium-explosive-smasher-dx-machine-slayer-oh-yes-edition");
    }

    public void openTwitter(){
        Application.OpenURL("https://twitter.com/Geo_Montauk");
    }

    public void openTumblr(){
        Application.OpenURL("https://maessu-mes.tumblr.com");
    }

    public void openInsta(){
        Application.OpenURL("https://www.instagram.com/maessu_mes/");
    }

    public void OpenGitHub(){
        Application.OpenURL("https://github.com/AlairTox");
    }

    public void OpenReddit(){
        Application.OpenURL("https://www.reddit.com/user/BorealCobra");
    }
}
