using System.ComponentModel;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] float moveSpeed;
    [SerializeField] float minX, minY, minZ, maxX, maxY, maxZ, padding;
    [SerializeField] Sprite[] Sprite;
    
    [Header("Fire")]
    [SerializeField] float fireRate;
    [SerializeField] PlayerProjectile projectilePrefab;
    [SerializeField] FireSpark FireSpark;
    [SerializeField] public Transform projectiles;

    Rigidbody rigidBody;
    Coroutine fireCoroutine;
    SpriteRenderer spriteActual;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody =  gameObject.GetComponent<Rigidbody>();
        spriteActual = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    private void Move()
    {
        float deltaX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime * 5;
        float deltaZ = -(Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime * 5);
        
        float newPosX = Mathf.Clamp(transform.position.x + deltaX, minX + padding, maxX - padding);
        float newPosZ = Mathf.Clamp(transform.position.z + deltaZ, minZ + padding, maxZ - padding);
        
        rigidBody.MovePosition(new UnityEngine.Vector3(newPosX, transform.position.y, newPosZ));        
    }
    void Fire(){
        if(Input.GetButtonDown("Fire1"))
            fireCoroutine = StartCoroutine(FireContinuosly());
        if(Input.GetButtonUp("Fire1"))
            StopCoroutine(fireCoroutine);
    }

    IEnumerator FireContinuosly(){
        yield return new WaitForSeconds(fireRate);
        FireSpark.ShowSpark();
        ShootProjectile(FireSpark.transform.position);
        fireCoroutine = StartCoroutine(FireContinuosly());
    }

    private void ShootProjectile(UnityEngine.Vector3 projectilePosition){
        var newProjectile = Instantiate(projectilePrefab, projectilePosition, UnityEngine.Quaternion.identity);
        newProjectile.transform.SetParent(projectiles);
    }
}

