
using System.Threading;
using System;
using System.Drawing;
using System.Data;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("Move")]
    [SerializeField] Sprite[] newSprite;
    [SerializeField] float force, maxVel;
    
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
        float deltaX = Input.GetAxis("Horizontal");
        float deltaZ = - Input.GetAxis("Vertical");
        Vector3 vector= new Vector3(deltaX, transform.position.y, deltaZ);
        rigidBody.AddForce(vector * force * Time.deltaTime);
        if(rigidBody.velocity.magnitude > maxVel){
            rigidBody.velocity = Vector3.Normalize(rigidBody.velocity) * maxVel; 
        }
        if(deltaX == 0 && deltaZ == 0){
            rigidBody.drag = 50f;
        }else{
            rigidBody.drag = 2f;
        }
    }

    void Fire(){
        //Si se presiona el botón de disparo se empiezan a generar proyectiles
        if(Input.GetButtonDown("Fire1"))
            fireCoroutine = StartCoroutine(FireContinuosly());
        if(fireCoroutine != null){
            //Si se deja de presionar el botón de disparo se detiene la generación de proyectiles
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
        //Si el botón de disparo sigue pulsado se hace otra llamada
        fireCoroutine = StartCoroutine(FireContinuosly());
    }

    private void ShootProjectile(UnityEngine.Vector3 projectilePosition){
        //Generación de un nuevo gameObject de tipo bala
        var newProjectile = Instantiate(projectilePrefab, projectilePosition, UnityEngine.Quaternion.identity);
        newProjectile.transform.SetParent(projectiles);
        AudioSource.PlayClipAtPoint(shootSFX, Camera.main.transform.position, shootSFXVolume);
        //Inicio de animación de disparo
        StartCoroutine(shootAnimation());
    }

    private void changeSprite(){
        //Obtención de posición del mouse para determinar el sprite que se debe usar 
        UnityEngine.Vector3 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        UnityEngine.Vector3 mouseOnScreen = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        UnityEngine.Vector3 direction = positionOnScreen - mouseOnScreen;
        float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //Si el jugador mira hacia atras
        switch(color){
            case 0:
                if(ang < -45 && ang >= -135){
                sprite.sprite = newSprite[3];
                    //Se coloca el return debido a que una vez cambiado el sprite la función debe terminar, al ser llamada desde update evita problemas de sobreposición
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
                    //Se coloca el return debido a que una vez cambiado el sprite la función debe terminar, al ser llamada desde update evita problemas de sobreposición
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
        //Si existe una colisión y el jugador ya no es invensible
        if(!isInvinsible){
            //Si la colisión ocurre con un enemigo(layer 7)
            if(collision.gameObject.layer == 7){
                AudioSource.PlayClipAtPoint(hitSFX, Camera.main.transform.position, hitSFXVolume);
                //Se le quita una vida al jugador
                FindObjectOfType<GameManager>().processDeath();
            }
        }
    }

    IEnumerator RemoveInvincibility()
    {
        //Espera de 3 segundos desde la aparición del jugador
        yield return new WaitForSeconds(3f);
        //Se destruye el componente Blinker del gameObject que da el parpadeo al sprite
        Destroy(GetComponent<Blinker>());
        //Se realiza esto para que el sprite se muestre a full color 
        GetComponent<SpriteRenderer>().color = UnityEngine.Color.white;
        //Se le quita la invensibilidad al jugador
        isInvinsible = false;
    }
    IEnumerator shootAnimation(){
        //Se obtiene la posición del mouse para determinar que animaición se debe mostrar
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
                    //Activación de animación a 0.25 de velcoidad
                    animator.Play("Base Layer.SmallBackWhite", 0, 0.25f);
                    //Se deja la animación correr por medio segundo
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
                    //Activación de animación a 0.25 de velcoidad
                    animator.Play("Base Layer.SmallBackBlack", 0, 0.25f);
                    //Se deja la animación correr por medio segundo
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

