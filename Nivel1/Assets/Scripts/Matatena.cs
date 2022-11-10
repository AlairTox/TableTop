using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matatena : MonoBehaviour
{
    [SerializeField] GameObject upgradePoint;
    [SerializeField] GameObject shadow;
    int random;
    GameObject shadowObj;
    Rigidbody rigidBody;

    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
        Vector3 shadowPosition = new Vector3(transform.position.x, 0, transform.position.z);
        shadowObj = Instantiate(shadow, shadowPosition, shadow.transform.rotation);
    }
    // Update is called once per frame
    void Update()
    {
        if(transform.position.y <= 0.5)
            rigidBody.isKinematic = false;
        
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
