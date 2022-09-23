using System.Transactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] int damage;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ProjectileMove();
        if((transform.position.z > 18 || transform.position.z < -17) || (transform.position.x < -20 || transform.position.z > 15)){
            Destroy(gameObject);
        }
    }

    void ProjectileMove(){
        float deltaZ = moveSpeed * Time.deltaTime;
        float newPosZ = transform.position.z + deltaZ;
        transform.position = new Vector3(transform.position.x, transform.position.y, newPosZ);
    }
}
