using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matatena : MonoBehaviour
{
    [SerializeField] Sprite[] actualSprite;
    
    [SerializeField] AudioClip deathSFX;
    [SerializeField] [Range(0,1)] float deathSFXVolume;

    [Header("Prefabs")]
    [SerializeField] GameObject upgradePoint;
    [SerializeField] GameObject shadow;

    int random;
    int chooseSprite;
    GameObject shadowObj;
    Rigidbody rigidBody;
    SpriteRenderer sprite;
    Animator animator;

    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        chooseSprite = Random.Range(0,6);
        sprite.sprite = actualSprite[chooseSprite];

        rigidBody = gameObject.GetComponent<Rigidbody>();
        Vector3 shadowPosition = new Vector3(transform.position.x, 0, transform.position.z);
        shadowObj = Instantiate(shadow, shadowPosition, shadow.transform.rotation);
        animator = gameObject.GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        shadowObj.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        if(transform.position.y <= 2){
            rigidBody.constraints = RigidbodyConstraints.FreezePositionY;
        }
        if((transform.position.z > 18 || transform.position.z < -15) || (transform.position.x < -30 || transform.position.z > 25)){
            Destroy(shadowObj);
            Destroy(gameObject);
        }
        
    }
    public void processHit(){
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSFXVolume);
         //Se obtine un número aleatorio entre  0 y 1
        random = Random.Range(0,2);
        if(random == 1)
            //Si el número obtenido es 1 se crea un objeto de tipo upgreadePoint
            Instantiate(upgradePoint, transform.position, upgradePoint.transform.rotation);
        FindObjectOfType<GameManager>().addToScore(100);
        StartCoroutine(deathMatatena());
        }

    void OnCollisionEnter(Collision collision){
        if((collision.gameObject.layer == 8) || (collision.gameObject.layer == 6))
            processHit();
    }

    IEnumerator deathMatatena(){        
        animator.enabled = true;
        switch(chooseSprite){
            case 0:
                animator.Play("Base Layer.Explosion_0", 0, 0.75f);
            break;
            case 1:
                animator.Play("Base Layer.Explosion_1", 0, 0.75f);
            break;
            case 2:
                animator.Play("Base Layer.Explosion_2", 0, 0.75f);
            break;
            case 3:
                animator.Play("Base Layer.Explosion_3", 0, 0.75f);
            break;
            case 4:
                animator.Play("Base Layer.Explosion_4", 0, 0.75f);
            break;
            case 5:
                animator.Play("Base Layer.Explosion_5", 0, 0.75f);
            break;
        }
        yield return new WaitForSeconds(0.15f);

        //Se destruye al enemigo
        Destroy(shadowObj);
        Destroy(gameObject);
    }
}
