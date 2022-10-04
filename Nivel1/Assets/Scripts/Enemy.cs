using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] int health;
    [SerializeField] int damage;

    void Start()
    {

    }

    void Update()
    {

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
