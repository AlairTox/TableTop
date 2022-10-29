
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] [Range(0,5)] float gameSpeed;
    [SerializeField] int lives;
    [SerializeField] GameObject[] prefabsPlayer;

    Transform position;
    GameObject player, oldPlayer;
    int upgradeLevel = 0;
    int upgradePoints = 0;
    int currentScore;
    int points;
    // Start is called before the first frame update
    void Start()
    {
        //Al inicio del juego se crea un peón
        player = Instantiate(prefabsPlayer[0], prefabsPlayer[0].transform.position, prefabsPlayer[0].transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale  = gameSpeed;
    }

    public int getScore(){
        return currentScore;
    }

    public void addToScore(int points){
        currentScore += points;
    }

    public void processDeath(){
        lives--;
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
            }
        }
    }
}
