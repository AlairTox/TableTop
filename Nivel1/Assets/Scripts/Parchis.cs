
using System.Threading;
using System.Transactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parchis : MonoBehaviour
{
    [SerializeField] GameObject upgradePoint;
    [SerializeField] Sprite[] actualSprite;


    [Header("Health")]
    [SerializeField] int health;
    [SerializeField] int damage;

    [Header("Audio")]
    [SerializeField] AudioClip deathAudio;
    [SerializeField] [Range(0, 1)] float deathAudioVolume;

    SpriteRenderer sprite;
    int random;
    Player player;
    Rigidbody rigidBody;
    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        random = Random.Range(0,2);
        sprite.sprite = actualSprite[random];
        rigidBody = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        //Se busca dentro de la escena un objeto de tipo Player
        player = FindObjectOfType<Player>();
        Move();
    }

    private void Move(){
        Vector3 distance = transform.position - player.transform.position;
        Vector3 perpendicular = Vector3.Cross(distance , Vector3.up).normalized;
        // Add a bit of direction towards the player
        // Get magnitude squared, negative to get opposite direction
        float distMag = distance.magnitude * -1f;
        // Normalize distance to get a direction vector
        distance.Normalize();

        // Create new combined speed vector
        // Perpendicular speed will always be the same
        Vector3 orbitSpeed = perpendicular * 7.5f;

        // If distance is too small it will never touch the character, thus have a hard minimum
        //UnityEngine.Debug.Log(distMag);
        //distMag = distMag < -3.5f ? distMag : -3.5f;
        // Ternary operator, basically a simplified if-else

        // Distance speed will change depending on distance, times a constant
        orbitSpeed += distance * distMag * 0.5f;

        rigidBody.velocity = orbitSpeed;

        /*if((distance.x < 1.5 && distance.x > -1.5) && (distance.z < 1.5 && distance.z > -1.5))
            Destroy(gameObject);*/
    }

    public void processHit(int damage){
        //Se le resta vida
        health -= damage;
        if(health == 0){
            //Se obtine un número aleatorio entre  0 y 1
            random = Random.Range(0,2);
            if(random == 1)
                //Si el número obtenido es 1 se crea un objeto de tipo upgreadePoint
                Instantiate(upgradePoint, transform.position, upgradePoint.transform.rotation);
            FindObjectOfType<GameManager>().addToScore(100);
            AudioSource.PlayClipAtPoint(deathAudio, Camera.main.transform.position, deathAudioVolume);    
            //Se destruye al enemigo
            Destroy(gameObject);
        }
    }
    //Si se detecta una colisión de culaquier tipo
    void OnCollisionEnter(Collision collision){
        processHit(damage);
    }

}
