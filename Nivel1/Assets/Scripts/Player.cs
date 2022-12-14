
using System;
using System.Drawing;
using System.Data;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("Move")]
    [SerializeField] float moveSpeed;
    [SerializeField] float minX, minZ, maxX, maxZ, padding;
    [SerializeField] Sprite[] newSprite;
    
    [Header("Fire")]
    [SerializeField] float fireRate;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] public Transform projectiles;
    
    [Header("SFX")]
    [SerializeField] AudioClip shootSFX;
    [SerializeField] [Range(0,1)] float shootSFXVolume;
    [SerializeField] AudioClip hitSFX;
    [SerializeField] [Range(0, 1)] float hitSFXVolume;

    int color;
    Animator animator;
    FireSpark FireSpark;
    Rigidbody rigidBody;
    Coroutine fireCoroutine;
    SpriteRenderer sprite;
    bool isInvinsible;
    Quaternion rotation0 = Quaternion.Euler(45, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        //Se obtiene el componente Animator para realizar las correspondientes animaciones y se desactiva
        animator = gameObject.GetComponent<Animator>();
        animator.enabled = false;
        //Se obtiene el FireSpark del gameObject
        FireSpark = FindObjectOfType<FireSpark>();
        //Se obtienen los componentes rigidbody y SpriteRenderer para hacer los cambios oportunos
        rigidBody = gameObject.GetComponent<Rigidbody>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        //Se le otorga invensibilidad y se inicia la rutina para removerla 
        isInvinsible = true;
        StartCoroutine(RemoveInvincibility());
        rigidBody.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        color = FindObjectOfType<GameManager>().getColor(); 
        Move();
        Fire();
        changeSprite();
    }

    private void Move()
    {
        //Se obtienen las teclas pulsadas por el jugador WASD para determinar hacia donde se movera
        float deltaX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime * 5;
        float deltaZ = -(Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime * 5);
        //Se calcula la nueva posici??n del jugador
        float newPosX = Mathf.Clamp(transform.position.x + deltaX, minX + padding, maxX - padding);
        float newPosZ = Mathf.Clamp(transform.position.z + deltaZ, minZ + padding, maxZ - padding);
        //Se desplaza el gameObject del jugador a la nueva posici??n
        rigidBody.MovePosition(new UnityEngine.Vector3(newPosX, transform.position.y, newPosZ));        

    }

    void Fire(){
        //Si se presiona el bot??n de disparo se empiezan a generar proyectiles
        if(Input.GetButtonDown("Fire1"))
            fireCoroutine = StartCoroutine(FireContinuosly());
        if(fireCoroutine != null){
            //Si se deja de presionar el bot??n de disparo se detiene la generaci??n de proyectiles
            if(Input.GetButtonUp("Fire1"))
                StopCoroutine(fireCoroutine);
        }
    }

    IEnumerator FireContinuosly(){
        //Se espera el delay ingresado entre proyectil
        yield return new WaitForSeconds(fireRate);
        FireSpark.ShowSpark();
        //Se genera el nuevo proyectil
        ShootProjectile(FireSpark.transform.position);
        //Si el bot??n de disparo sigue pulsado se hace otra llamada
        fireCoroutine = StartCoroutine(FireContinuosly());
    }

    private void ShootProjectile(UnityEngine.Vector3 projectilePosition){
        //Generaci??n de un nuevo gameObject de tipo bala
        var newProjectile = Instantiate(projectilePrefab, projectilePosition, UnityEngine.Quaternion.identity);
        newProjectile.transform.SetParent(projectiles);
        AudioSource.PlayClipAtPoint(shootSFX, Camera.main.transform.position, shootSFXVolume);
        //Inicio de animaci??n de disparo
        StartCoroutine(shootAnimation());
    }

    private void changeSprite(){
        //Obtenci??n de posici??n del mouse para determinar el sprite que se debe usar 
        UnityEngine.Vector3 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        UnityEngine.Vector3 mouseOnScreen = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        UnityEngine.Vector3 direction = positionOnScreen - mouseOnScreen;
        float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //Si el jugador mira hacia atras
        switch(color){
            case 0:
                if(ang < -45 && ang >= -135){
                sprite.sprite = newSprite[3];
                    //Se coloca el return debido a que una vez cambiado el sprite la funci??n debe terminar, al ser llamada desde update evita problemas de sobreposici??n
                    return;
                }
                //Si el jugador mira hacia delante
                if(ang > 45 && ang <= 135){
                    sprite.sprite = newSprite[1];
                    return;
                }        
                //Si el jugador mira hacia la derecha
                if(ang > 135 || ang < -135){
                    sprite.sprite = newSprite[0];
                    return;
                }
                //Si el jugador mira hacia la izquierda
                if(ang > -45 && ang <= 45){
                    sprite.sprite = newSprite[2];
                    return;
                }
            break;
            case 1:
                if(ang < -45 && ang >= -135){
                sprite.sprite = newSprite[7];
                    //Se coloca el return debido a que una vez cambiado el sprite la funci??n debe terminar, al ser llamada desde update evita problemas de sobreposici??n
                    return;
                }
                //Si el jugador mira hacia delante
                if(ang > 45 && ang <= 135){
                    sprite.sprite = newSprite[5];
                    return;
                }        
                //Si el jugador mira hacia la derecha
                if(ang > 135 || ang < -135){
                    sprite.sprite = newSprite[4];
                    return;
                }
                //Si el jugador mira hacia la izquierda
                if(ang > -45 && ang <= 45){
                    sprite.sprite = newSprite[6];
                    return;
                }
            break;
        }
    }

    void OnCollisionEnter(Collision collision){
        //Si existe una colisi??n y el jugador ya no es invensible
        if(!isInvinsible){
            //Si la colisi??n ocurre con un enemigo(layer 7)
            if(collision.gameObject.layer == 7){
                AudioSource.PlayClipAtPoint(hitSFX, Camera.main.transform.position, hitSFXVolume);
                //Se le quita una vida al jugador
                FindObjectOfType<GameManager>().processDeath();
            }
        }
    }

    IEnumerator RemoveInvincibility()
    {
        //Espera de 3 segundos desde la aparici??n del jugador
        yield return new WaitForSeconds(3f);
        //Se destruye el componente Blinker del gameObject que da el parpadeo al sprite
        Destroy(GetComponent<Blinker>());
        //Se realiza esto para que el sprite se muestre a full color 
        GetComponent<SpriteRenderer>().color = UnityEngine.Color.white;
        //Se le quita la invensibilidad al jugador
        isInvinsible = false;
    }
    IEnumerator shootAnimation(){
        //Se obtiene la posici??n del mouse para determinar que animaici??n se debe mostrar
        UnityEngine.Vector3 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        UnityEngine.Vector3 mouseOnScreen = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        UnityEngine.Vector3 direction = positionOnScreen - mouseOnScreen;
        float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //Se activa el componente Animator
        animator.enabled = true;
        switch(color){
            case 0:
                //Si el jugador mira hacia atras
                if(ang < -45 && ang >= -135){
                    //Activaci??n de animaci??n a 0.25 de velcoidad
                    animator.Play("Base Layer.SmallBackWhite", 0, 0.25f);
                    //Se deja la animaci??n correr por medio segundo
                    yield return new WaitForSeconds(0.5f);
                    //Se desactiva el componente Animator
                    animator.enabled = false;
                }
                //Si el jugador mira hacia delante
                if(ang > 45 && ang <= 135){
                    animator.Play("Base Layer.SmallFrontWhite", 0, 0.25f);
                    yield return new WaitForSeconds(0.5f);
                    animator.enabled = false;
                }        
                //Si el jugador mira hacia la derecha
                if(ang > 135 || ang < -135){
                    animator.Play("Base Layer.SmallRightWhite", 0, 0.25f);
                    yield return new WaitForSeconds(0.5f);
                    animator.enabled = false;
                }
                //Si el jugador mira hacia la izquierda
                if(ang > -45 && ang <= 45){
                    animator.Play("Base Layer.SmallLeftWhite", 0, 0.25f);
                    yield return new WaitForSeconds(0.5f);
                    animator.enabled = false;
                }
            break;
            case 1:
                //Si el jugador mira hacia atras
                if(ang < -45 && ang >= -135){
                    //Activaci??n de animaci??n a 0.25 de velcoidad
                    animator.Play("Base Layer.SmallBackBlack", 0, 0.25f);
                    //Se deja la animaci??n correr por medio segundo
                    yield return new WaitForSeconds(0.5f);
                    //Se desactiva el componente Animator
                    animator.enabled = false;
                }
                //Si el jugador mira hacia delante
                if(ang > 45 && ang <= 135){
                    animator.Play("Base Layer.SmallFrontBlack", 0, 0.25f);
                    yield return new WaitForSeconds(0.5f);
                    animator.enabled = false;
                }        
                //Si el jugador mira hacia la derecha
                if(ang > 135 || ang < -135){
                    animator.Play("Base Layer.SmallRightBlack", 0, 0.25f);
                    yield return new WaitForSeconds(0.5f);
                    animator.enabled = false;
                }
                //Si el jugador mira hacia la izquierda
                if(ang > -45 && ang <= 45){
                    animator.Play("Base Layer.SmallLeftBlack", 0, 0.25f);
                    yield return new WaitForSeconds(0.5f);
                    animator.enabled = false;
                }
            
            break;
        }
    }
}

