
using System.Diagnostics;
using System;
using System.Threading;
using System.Transactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] int health;
    [SerializeField] int damage;
    
    [Header("Move")]
    [SerializeField] Transform player;

    Rigidbody rigidBody;
    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
    }

    private void Move(){
        UnityEngine.Vector3 distance = transform.position - player.position;
        Vector3 perpendicular = Vector3.Cross(distance , Vector3.up).normalized;
        UnityEngine.Debug.Log(distance);
        if(perpendicular.z < 0 && perpendicular.x < 0)
            perpendicular.z += -0.1f;
        if(perpendicular.x > 0 && perpendicular.z < 0)
            perpendicular.x += 0.1f;
        if(perpendicular.z > 0 && perpendicular.x > 0)
            perpendicular.z += 0.1f;
        if(perpendicular.x < 0 && perpendicular.z > 0)
            perpendicular.x += -0.1f;
        rigidBody.velocity = perpendicular * 10;
        if((distance.x < 1.5 && distance.x > -1.5) && (distance.z < 1.5 && distance.z > -1.5))
            Destroy(gameObject);
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
