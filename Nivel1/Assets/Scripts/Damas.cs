
using System.Security.AccessControl;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Damas : MonoBehaviour
{

    [Header("Health")]
    [SerializeField] int health;
    [SerializeField] int damage;

    Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
        StartCoroutine(Jump());
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }


    private void Move(){
        //Movimiento hacia delante
        rigidBody.velocity = new Vector3(0,0,-5);
    }

    public void processHit(int damage){
        health -= damage;
        if(health == 0)
            Destroy(gameObject);
    }
    void OnCollisionEnter(Collision collision){
        processHit(damage);
    }
    IEnumerator Jump(){
        //Tiempo de espera entre salto
        yield return new WaitForSeconds(4f);
        //Salto, impulso de 10 en Y y -5 en Z para que pueda rebasar a la pareja, regresa al suelo debido a la gravedad  
        rigidBody.AddForce(0,10,-5, ForceMode.Impulse);
        StartCoroutine(Jump());
    }
}
