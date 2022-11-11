using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePoints : MonoBehaviour
{
    void OnCollisionEnter(Collision collision){
        //Si la colision ocurre con una bala(layer 8) o con el jugador(layer 6)
        if((collision.gameObject.layer == 8) || (collision.gameObject.layer == 6)){
            Destroy(gameObject);
            //Se envía una llamada al gameManager en donde se le aumentara en uno el puntaje de upgrade
            FindObjectOfType<GameManager>().changePlayer();
        }
    }
}
