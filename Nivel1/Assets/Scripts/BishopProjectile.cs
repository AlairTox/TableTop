
using System.Security.AccessControl;
using System.IO;
using System.Security.Cryptography;
using System;
using System.Threading;
using System.Transactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BishopProjectile : MonoBehaviour
{
    [SerializeField] int damage;
    Rigidbody rigidBody;
    Coroutine enableCollider;
    Collider projectileCollider;    

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
        ProjectileMove();
        projectileCollider = GetComponent<Collider>();        
        enableCollider = StartCoroutine(disableTrigger());
    }

    // Update is called once per frame
    void Update()
    {
        if((transform.position.z > 30 || transform.position.z < -30) || (transform.position.x < -30 || transform.position.z > 30))
            Destroy(gameObject);
    }

    void ProjectileMove(){

        UnityEngine.Vector3 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        UnityEngine.Vector3 mouseOnScreen = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        UnityEngine.Vector3 direction = positionOnScreen - mouseOnScreen;
        float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if(ang <= -90)
            rigidBody.AddForce(15, 0, 15, ForceMode.Impulse);
        if(ang > -90 && ang <= 0)
            rigidBody.AddForce(-15, 0, 15, ForceMode.Impulse);
        if(ang > 0 && ang <= 90)
            rigidBody.AddForce(-15, 0, -15, ForceMode.Impulse);
        if(ang > 90 && ang <= 180)
            rigidBody.AddForce(15, 0, -15, ForceMode.Impulse);
    }

    IEnumerator disableTrigger(){
        yield return new WaitForSeconds(.2f);
        projectileCollider.isTrigger = false;
        StopCoroutine(disableTrigger());
    }
    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.tag != "Player"){
            Destroy(gameObject);
        }
    }
}
