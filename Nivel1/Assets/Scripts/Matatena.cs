using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matatena : MonoBehaviour
{
    [SerializeField] GameObject upgradePoint;
    [SerializeField] GameObject shadow;
    [SerializeField] Sprite[] actualSprite;
    int random;
    GameObject shadowObj;
    Rigidbody rigidBody;
    SpriteRenderer sprite;

    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        random = Random.Range(0,6);
        sprite.sprite = actualSprite[random];

        rigidBody = gameObject.GetComponent<Rigidbody>();
        Vector3 shadowPosition = new Vector3(transform.position.x, 0, transform.position.z);
        shadowObj = Instantiate(shadow, shadowPosition, shadow.transform.rotation);
    }
    // Update is called once per frame
    void Update()
    {
        if(transform.position.y <= 2)
            rigidBody.constraints = RigidbodyConstraints.FreezePositionY;
        if((transform.position.z > 50 || transform.position.z < -30) || (transform.position.x < -30 || transform.position.z > 30)){
            Destroy(shadowObj);
            Destroy(gameObject);
        }
        
    }
    public void processHit(){
         //Se obtine un número aleatorio entre  0 y 1
        random = Random.Range(0,2);
        if(random == 1)
            //Si el número obtenido es 1 se crea un objeto de tipo upgreadePoint
            Instantiate(upgradePoint, transform.position, upgradePoint.transform.rotation);
        //Se destruye al enemigo
        Destroy(shadowObj);
        Destroy(gameObject);
        }
    void OnCollisionEnter(Collision collision){
        if((collision.gameObject.layer == 8) || (collision.gameObject.layer == 6))
            processHit();
    }
}
