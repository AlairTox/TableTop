using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] [Range(0,5)] float gameSpeed;
    [SerializeField] int lives;
    [SerializeField] Player player;

    int currentScore;
    int points;
    // Start is called before the first frame update
    void Start()
    {

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
}
