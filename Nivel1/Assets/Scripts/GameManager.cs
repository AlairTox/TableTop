
using System.Timers;
using System.Threading;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] GameObject[] prefabsPlayer;
    
    [Header("Game")]
    [SerializeField] [Range(0,5)] float gameSpeed;
    [SerializeField] public int lifes;
    [SerializeField] Text scoreText; 
    [SerializeField] Text upgradeText;
    [SerializeField] public int color;

    [Header("Music")]
    [SerializeField] AudioClip gameMusic;
    [SerializeField] AudioClip startMusic;
    [SerializeField] AudioClip gameOverMusic;
    [SerializeField] [Range(0, 1)] float gameMusicVolume;
    [SerializeField] [Range(0, 1)] float startMusicVolume;
    [SerializeField] [Range(0, 1)] float gameOverMusicVolume;

    [Header("Pause")]
    [SerializeField] private GameObject menuPausa;
    [SerializeField] private GameObject gameOverUI;

    Transform position;
    Boolean gameIsPaused;
    GameObject player, oldPlayer;
    int upgradeLevel = 0;
    public int upgradePoints = 0;
    public int currentScore;
    private AudioSource audioSource;

    Vector3 rotationVector = new Vector3(0, -90, -45);
    Vector3 rotationVectorFinish = new Vector3(45, 0, 0);
    Quaternion rotationInit;
    float fov = 39.0f;
    private string colorPrefsName = "color";
    // Start is called before the first frame update
     void Awake()
    {
        LoadData();    
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();   
        StartCoroutine(changeFOV());
        Vector3 positionUpgrade = new Vector3(0, upgradeText.transform.position.y, upgradeText.transform.position.z);
        rotationInit = Quaternion.Euler(rotationVector);
        upgradeText.fontSize = scoreText.fontSize = 48;
        upgradeText.transform.position = positionUpgrade;
        //Al inicio del juego se crea un peón
        player = Instantiate(prefabsPlayer[0], prefabsPlayer[0].transform.position, prefabsPlayer[0].transform.rotation);
        Time.timeScale  = gameSpeed;
        audioSource.clip = startMusic;
        audioSource.Play();
        StartCoroutine(playMusic());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            gameIsPaused = !gameIsPaused;
            PauseGame();
        }
        scoreText.text = "\t" + currentScore.ToString();
        upgradeText.text = upgradePoints.ToString() + "\t";
        
    }

    public void addToScore(int points){
        currentScore += points;
    }
    public int getColor(){
        return color;
    }
    public int getLifes(){
        return lifes;
    }

    public void processDeath(){
        FindObjectOfType<sistemaVidas>().changeLifes();
        //Se restablece a 0 los puntos de upgrade
        upgradePoints = 0;
        //Se resta en uno el nivel de upgrade
        upgradeLevel--;
        //Obtención de la posición del jugador
        position = player.transform;
        //Si el jugador murió siendo un peón entonces se suma en uno el nivel de upgrade para que vuelva a ser un peón
        if(upgradeLevel == -1)
            upgradeLevel++;
        //Se destruye el jugador actual para crear inmediatamente uno nuevo con las características actualizadas
        Destroy(player);
        player = Instantiate(prefabsPlayer[upgradeLevel], position.position, prefabsPlayer[upgradeLevel].transform.rotation);
        //StartCoroutine(changeRotation());
        //Se resta una vida al jugador
        lifes--;
        if(lifes == 0){
            audioSource.Stop();
            gameOverUI.SetActive(true);
            StartCoroutine(gameOver());
        }
    }

    public void changePlayer(){
        //Se suma uno al puntaje de upgrade
        upgradePoints++;
        //Se verifica si no ha llegado al último nivel de upgrade
        if(upgradeLevel < 4){
                //Si se obtuvieron 10 puntos de upgrade se obtiene el cambio de prefab
            switch(upgradeLevel){
                case 0:
                    if(upgradePoints == 3){
                        //Se resetean los puntos para poder hacer el siguiente upgrade
                        upgradePoints = 0;
                        //Se obtiene el indice del nuevo prefab
                        upgradeLevel++; 
                        //Se obtiene la última posición del jugador para que el nuevo prefab sea colocado en ese sitio
                        position = player.transform;
                        //Se destruye el prefab anterior
                        Destroy(player);
                        //Se crea el nuevo prefab con el upgrade realizado
                        player = Instantiate(prefabsPlayer[upgradeLevel], position.position, prefabsPlayer[upgradeLevel].transform.rotation);
                        //StartCoroutine(changeRotation());
                    }
                break;
                case 1:
                    if(upgradePoints == 3){
                        upgradePoints = 0;
                        upgradeLevel++; 
                        position = player.transform;
                        Destroy(player);
                        player = Instantiate(prefabsPlayer[upgradeLevel], position.position, prefabsPlayer[upgradeLevel].transform.rotation);
                    }
                break;
                case 2:
                    if(upgradePoints == 5){
                        upgradePoints = 0;
                        upgradeLevel++; 
                        position = player.transform;
                        Destroy(player);
                        player = Instantiate(prefabsPlayer[upgradeLevel], position.position, prefabsPlayer[upgradeLevel].transform.rotation);
                    }
                break;
                case 3:
                    if(upgradePoints == 9){
                        upgradePoints = 0;
                        upgradeLevel++; 
                        position = player.transform;
                        Destroy(player);
                        player = Instantiate(prefabsPlayer[upgradeLevel], position.position, prefabsPlayer[upgradeLevel].transform.rotation);
                    }
                break;
            }
            
        }
    }

    void PauseGame (){
        if(gameIsPaused){
            Time.timeScale = 0f;
            menuPausa.SetActive(true);
        }else{
            Time.timeScale = gameSpeed;
            menuPausa.SetActive(false);
        } 
    }

    IEnumerator changeFOV(){
        yield return new WaitForSeconds(0.03f);
        if(fov != Camera.main.fieldOfView){
            Camera.main.fieldOfView--;
            StartCoroutine(changeFOV());
        }
    }
    IEnumerator playMusic(){
        yield return new WaitForSeconds(7f);
        audioSource.clip = gameMusic;
        audioSource.Play();
    }

    private void LoadData(){
        color = PlayerPrefs.GetInt(colorPrefsName, 0);
    }

    IEnumerator gameOver(){
        Destroy(player);
        audioSource.clip = gameOverMusic;
        audioSource.Play();
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    // IEnumerator changeRotation(){
    //     yield return new WaitForSeconds(0.2f);
    //     Quaternion finishRotation = Quaternion.Euler(rotationVectorFinish);
    //     player.transform.rotation = Quaternion.Slerp(player.transform.rotation, finishRotation,  Time.deltaTime * 0.5f);

    // }
}

