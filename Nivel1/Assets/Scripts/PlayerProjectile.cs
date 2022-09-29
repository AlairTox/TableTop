
using System.Security.AccessControl;
using System.IO;
using System.Security.Cryptography;
using System;
using System.Threading;
using System.Transactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] int damage;
    Rigidbody rigidBody;
    float newPosZ, newPosX, deltaX, deltaZ;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
        KeyLogger();
        ProjectileMove();
    }

    // Update is called once per frame
    void Update()
    {
        if((transform.position.z > 18 || transform.position.z < -17) || (transform.position.x < -20 || transform.position.z > 15))
            Destroy(gameObject);
    }

    void KeyLogger(){
       deltaZ = Input.GetAxis("Vertical");
       deltaX = Input.GetAxis("Horizontal");
    }

    void ProjectileMove(){
        if(deltaZ < 0){
            rigidBody.AddForce(0, 0, 20, ForceMode.Impulse);
            return;
        }
        if(deltaZ > 0){
            rigidBody.AddForce(0, 0, -20, ForceMode.Impulse);
            return;
        }
        if(deltaX > 0){
            rigidBody.AddForce(20, 0, 0, ForceMode.Impulse);
            return;
        }
        if(deltaX < 0){
            rigidBody.AddForce(-20, 0, 0, ForceMode.Impulse);
            return;
        }
        if(deltaX == 0 && deltaZ == 0)
            rigidBody.AddForce(0, 0, -20, ForceMode.Impulse);
    }
}
