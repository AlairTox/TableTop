
using System.Security.AccessControl;
using System.IO;
using System.Security.Cryptography;
using System;
using System.Threading;
using System.Transactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnProjectile : MonoBehaviour
{
    Rigidbody rigidBody;
    Coroutine enableCollider;
    Collider projectileCollider;    
    Coroutine trajectory;

    // Start is called before the first frame update
    void Start()
    {
        //Se obtiene el RigidBody  y se le otorga el movimiento correspondiente
        rigidBody = gameObject.GetComponent<Rigidbody>();
        ProjectileMove();
        projectileCollider = GetComponent<Collider>();        
        enableCollider = StartCoroutine(disableTrigger());
    }

    // Update is called once per frame
    void Update()
    {
        //Si el proyectil sale del campo de juego se destruye
        if((transform.position.z > 30 || transform.position.z < -30) || (transform.position.x < -30 || transform.position.z > 30))
            Destroy(gameObject);
    }

    void ProjectileMove(){
        //Se obtiene la posición del mouse al momento de generar el proyectil
        UnityEngine.Vector3 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        UnityEngine.Vector3 mouseOnScreen = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        UnityEngine.Vector3 direction = positionOnScreen - mouseOnScreen;
        float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //Se realizan los ifs correspondientes para dirigr el proyectil
        if(ang < -45 && ang >= -135){
            rigidBody.AddForce(0, 0, 10, ForceMode.Impulse);
            //Se inicia una rutina por el movimiento especial en la bala del peón
            trajectory = StartCoroutine(changeMove());
        }
        if(ang > 45 && ang <= 135){
            rigidBody.AddForce(0, 0, -10, ForceMode.Impulse);
            trajectory = StartCoroutine(changeMove());
        }
        if(ang > 135 || ang < -135){
            rigidBody.AddForce(10, 0, 0, ForceMode.Impulse);
            trajectory = StartCoroutine(changeMove());
        }
        if(ang > -45 && ang <= 45){
            rigidBody.AddForce(-10, 0, 0, ForceMode.Impulse);
            trajectory = StartCoroutine(changeMove());
        }
    }
    IEnumerator changeMove(){
        //Después de un segunndo existiendo el proyectil es eliminado
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
        StopCoroutine(changeMove());
    }
    //Se realiza esto para que el collider del proyectil no de con el jugador y la fuerza generada no provoque un impulso
    IEnumerator disableTrigger(){
        yield return new WaitForSeconds(.2f);
        projectileCollider.isTrigger = false;
        StopCoroutine(disableTrigger());
    }
    //Si la colisión ocurre con cualquier otra cosa que no sea el jugador el proyectil se destruye
    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.tag != "Player"){
            Destroy(gameObject);
        }
    }    
}
