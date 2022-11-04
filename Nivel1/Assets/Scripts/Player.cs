
using System.Drawing;
using System.Data;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] GameObject[] PlayerPrefabs;
    [SerializeField] int health;

    [Header("Move")]
    [SerializeField] float moveSpeed;
    [SerializeField] float minX, minY, minZ, maxX, maxY, maxZ, padding;
    [SerializeField] Sprite[] newSprite;
    
    [Header("Fire")]
    [SerializeField] float fireRate;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] public Transform projectiles;

    FireSpark FireSpark;
    Rigidbody rigidBody;
    Coroutine fireCoroutine;
    SpriteRenderer sprite;
    bool isInvinsible;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        FireSpark = FindObjectOfType<FireSpark>();
        rigidBody =  gameObject.GetComponent<Rigidbody>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        isInvinsible = true;
        StartCoroutine(RemoveInvincibility());
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
        changeSprite();
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
        if(fireCoroutine != null){
            if(Input.GetButtonUp("Fire1"))
                StopCoroutine(fireCoroutine);
        }
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

    private void changeSprite(){
        UnityEngine.Vector3 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        UnityEngine.Vector3 mouseOnScreen = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        UnityEngine.Vector3 direction = positionOnScreen - mouseOnScreen;
        float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if(ang < -45 && ang >= -135){
           sprite.sprite = newSprite[3];
            return;
        }
        if(ang > 45 && ang <= 135){
            sprite.sprite = newSprite[1];
            return;
        }        
        if(ang > 135 || ang < -135){
            sprite.sprite = newSprite[0];
            return;
        }
        if(ang > -45 && ang <= 45){
            sprite.sprite = newSprite[2];
            return;
        }
    }

    void OnCollisionEnter(Collision collision){
        if(!isInvinsible){
            if(collision.gameObject.layer == 7)
                FindObjectOfType<GameManager>().processDeath();
        }
    }

    IEnumerator RemoveInvincibility()
    {
        yield return new WaitForSeconds(3f);
        Destroy(GetComponent<Blinker>());
        GetComponent<SpriteRenderer>().color = UnityEngine.Color.white;
        isInvinsible = false;
    }
}

