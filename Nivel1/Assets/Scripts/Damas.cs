
using System.Security.AccessControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damas : MonoBehaviour
{

    [SerializeField] GameObject upgradePoint; 

    [Header("Health")]
    [SerializeField] int health;
    [SerializeField] int damage;

    int random;
    Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }


    private void Move(){
        //Movimiento hacia delante
        rigidBody.velocity = new Vector3(0,0,-4);
    }

    public void processHit(int damage){
        health -= damage;
        if(health == 0){
            //Se obtiene un número aleatorio entre 0 y 1
            random = Random.Range(0,2);
            if(random == 1)
                //Si el número obtenido es 1 se crea un objeto de tipo upgradePoint
                Instantiate(upgradePoint, transform.position, upgradePoint.transform.rotation);
            //Se destruye al enemigo(ya no tiene vida)
            Destroy(gameObject);
        }

    }
    void OnCollisionEnter(Collision collision){
        if((collision.gameObject.layer == 8) || (collision.gameObject.layer == 6))
            processHit(damage);
    }

}
