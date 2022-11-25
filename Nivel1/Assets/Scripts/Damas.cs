
using System.Security.AccessControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damas : MonoBehaviour
{
    [SerializeField] GameObject upgradePoint; 
    [SerializeField] int color;

    [Header("Health")]
    [SerializeField] int health;
    [SerializeField] int damage;

    int random;
    Rigidbody rigidBody;
    SpriteRenderer spriteRenderer;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        rigidBody = gameObject.GetComponent<Rigidbody>();
        animator = gameObject.GetComponent<Animator>();
        animator.enabled = true;
        StartCoroutine(shootAnimation());
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if((transform.position.z > 30 || transform.position.z < -20) || (transform.position.x < -30 || transform.position.z > 30))
            Destroy(gameObject);
        
    }

    private void Move(){
        //Movimiento hacia delante
        rigidBody.velocity = new Vector3(0,0,-5);
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

    IEnumerator shootAnimation(){
        switch(color){
            case 0:
                animator.Play("Base Layer.DamaRoja", 0, 0.25f);
                yield return new WaitForSeconds(0.3f);
                StartCoroutine(shootAnimation());
            break;
            case 1:
                animator.Play("Base Layer.Damarilla", 0, 0.25f);
                yield return new WaitForSeconds(0.3f);
                StartCoroutine(shootAnimation());
            break;
            case 2:
                animator.Play("Base Layer.DamaVerde", 0, 0.25f);
                yield return new WaitForSeconds(0.3f);
                StartCoroutine(shootAnimation());
            break;
            case 3:
                animator.Play("Base Layer.Damazul", 0, 0.25f);
                yield return new WaitForSeconds(0.3f);
                StartCoroutine(shootAnimation());
            break;
            case 4:
                animator.Play("Base Layer.Damorada", 0, 0.25f);
                yield return new WaitForSeconds(0.3f);
                StartCoroutine(shootAnimation());
            break;

        }
    }

}
