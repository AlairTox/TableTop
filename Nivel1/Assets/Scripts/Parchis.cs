
using System.Diagnostics;
using System;
using System.Threading;
using System.Transactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parchis : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] int health;
    [SerializeField] int damage;
    
    [Header("Move")]

    Player player;

    Rigidbody rigidBody;
    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
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
        Vector3 orbitSpeed = perpendicular * 10;

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
        health -= damage;
        if(health == 0)
            Destroy(gameObject);
    }
    void OnCollisionEnter(Collision collision){
        processHit(damage);
    }

}
