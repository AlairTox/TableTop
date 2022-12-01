
using System.ComponentModel;
using System.Timers;
using System.Threading;
using System.Net.Mime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] GameObject[] prefabsPlayer;
    
    [Header("Game")]
    [SerializeField] [Range(0,5)] float gameSpeed;
    [SerializeField] public int lifes;
    [SerializeField] Text scoreText; 
    [SerializeField] Text upgradeText;
    [SerializeField] Text lifesText;
    [SerializeField] public int color;

    [Header("Music")]
    [SerializeField] AudioClip gameMusic;
    [SerializeField] [Range(0, 1)] float gameMusicVolume;

    Transform position;
    Boolean gameIsPaused;
    GameObject player, oldPlayer;
    int upgradeLevel = 0;
    public int upgradePoints = 0;
    public int currentScore;

    Vector3 rotationVector = new Vector3(0, -90, -45);
    Vector3 rotationVectorFinish = new Vector3(45, 0, 0);
    Quaternion rotationInit;
    // Start is called before the first frame update
    void Start()
    {
        rotationInit = Quaternion.Euler(rotationVector);
        upgradeText.fontSize = scoreText.fontSize = lifesText.fontSize = 28;
        //Al inicio del juego se crea un peón
        player = Instantiate(prefabsPlayer[0], prefabsPlayer[0].transform.position, prefabsPlayer[0].transform.rotation);
        Time.timeScale  = gameSpeed;
        //AudioSource.PlayClipAtPoint(gameMusic, Camera.main.transform.position, gameMusicVolume);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            gameIsPaused = !gameIsPaused;
            PauseGame();
        }
        scoreText.text = "\tScore: " + currentScore.ToString();
        upgradeText.text = "UpgradePoints: " + upgradePoints.ToString();
        lifesText.text = "\tHP: " + lifes.ToString();
        
        if(Input.GetKeyDown(KeyCode.Space))
            changeColor();
        
    }

    public void addToScore(int points){
        currentScore += points;
    }
    public int getColor(){
        return color;
    }

    public void processDeath(){
        //Se restablece a 0 los puntos de upgrade
        upgradePoints = 0;
        //Se resta en uno el nivel de upgrade
        upgradeLevel--;
        //Obtención de la posición del jugador
        position = player.transform;
        //Si el jugador murió siendo un peón entonces se suma en uno el nivel de upgrade para que vuelva a ser un peón
        if(upgradeLevel == -1)
            upgradeLevel++;
        //Se destruye el jugador actual para crear inmediatamente uno nuevo con las características actualizadas
        Destroy(player);
        player = Instantiate(prefabsPlayer[upgradeLevel], position.position, prefabsPlayer[upgradeLevel].transform.rotation);
        //StartCoroutine(changeRotation());
        //Se resta una vida al jugador
        lifes--;
        if(lifes == 0){
            //Game Over
            Time.timeScale = 0f;
            Destroy(player);
        }
    }

    public void changePlayer(){
        //Se suma uno al puntaje de upgrade
        upgradePoints++;
        //Se verifica si no ha llegado al último nivel de upgrade
        if(upgradeLevel < 4){
                //Si se obtuvieron 10 puntos de upgrade se obtiene el cambio de prefab
            if(upgradePoints == 2){
                //Se resetean los puntos para poder hacer el siguiente upgrade
                upgradePoints = 0;
                //Se obtiene el indice del nuevo prefab
                upgradeLevel++; 
                //Se obtiene la última posición del jugador para que el nuevo prefab sea colocado en ese sitio
                position = player.transform;
                //Se destruye el prefab anterior
                Destroy(player);
                //Se crea el nuevo prefab con el upgrade realizado
                player = Instantiate(prefabsPlayer[upgradeLevel], position.position, prefabsPlayer[upgradeLevel].transform.rotation);
                //StartCoroutine(changeRotation());
            }
        }
    }
    void PauseGame (){
        if(gameIsPaused)
            Time.timeScale = 0f;
        else 
            Time.timeScale = gameSpeed;
    }
    void changeColor(){
        if(color == 0)
            color = 1;
        else 
            color = 0;
    }

    // IEnumerator changeRotation(){
    //     yield return new WaitForSeconds(0.2f);
    //     Quaternion finishRotation = Quaternion.Euler(rotationVectorFinish);
    //     player.transform.rotation = Quaternion.Slerp(player.transform.rotation, finishRotation,  Time.deltaTime * 0.5f);

    // }
}
