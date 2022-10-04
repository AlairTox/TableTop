
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
    [SerializeField] int damage;
    Rigidbody rigidBody;
    Coroutine enableCollider;
    Collider projectileCollider;    
    Coroutine trajectory;

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

        if(ang < -45 && ang >= -135){
            rigidBody.AddForce(0, 0, 10, ForceMode.Impulse);
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
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
        StopCoroutine(changeMove());
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
