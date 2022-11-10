
using System.Security.AccessControl;
using System.IO;
using System.Security.Cryptography;
using System;
using System.Threading;
using System.Transactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RookProjectile : MonoBehaviour
{
    Rigidbody rigidBody;
    Coroutine enableCollider;
    Collider projectileCollider;    

    // Start is called before the first frame update
    void Start()
    {
        //Se obtiene el RigidBody del proyectil y se le otorga el correspondiente movimiento.
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
        //Se obtiene la posición del mouse en el momento de generación de proyectil
        UnityEngine.Vector3 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        UnityEngine.Vector3 mouseOnScreen = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        UnityEngine.Vector3 direction = positionOnScreen - mouseOnScreen;
        float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //Se le otorga la dirección de movimiento al proyectil según sea la posición del mouse
        if(ang < -45 && ang >= -135)
            rigidBody.AddForce(0, 0, 15, ForceMode.Impulse);
        if(ang > 45 && ang <= 135)
            rigidBody.AddForce(0, 0, -15, ForceMode.Impulse);
        if(ang > 135 || ang < -135)
            rigidBody.AddForce(15, 0, 0, ForceMode.Impulse);
        if(ang > -45 && ang <= 45)
            rigidBody.AddForce(-15, 0, 0, ForceMode.Impulse);
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
