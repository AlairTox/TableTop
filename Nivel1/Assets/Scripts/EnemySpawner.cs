
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] float delayTime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartEnemyCoroutines());
    }

    // Update is called once per frame
    void Update()
    {
    }

    void CreateEnemy(GameObject prefab, UnityEngine.Vector3 position){
        //Creación de un nuevo objeto(Enemigo)
        GameObject newEnemy = Instantiate(prefab, position, Quaternion.identity);
    }


    IEnumerator StartEnemyCoroutines(){
        //Tiempo de espera entre que inicia el juego y empiezan a hacer spawn los enemigos
        yield return new WaitForSeconds(delayTime);
        //Inicio de spawn
        StartCoroutine(SpawnParchis());
        StartCoroutine(SpawnDamas());
    }
    
    IEnumerator SpawnParchis(){
        //Tiempo de espera entre la aparición de Parchis
        yield return new WaitForSeconds(3f);
        //Obtención de una ubicación aleatoria dentro del campo de juego
        UnityEngine.Vector3 position = new UnityEngine.Vector3(Random.Range(-12, 12), 2f, Random.Range(-11, 22));
        CreateEnemy(enemyPrefabs[0], position);
        //Creación de Parchis nuevamente
        StartCoroutine(SpawnParchis());
    }
    IEnumerator SpawnDamas(){
        //Tiempo de espera entre la aparición de Parchis
        yield return new WaitForSeconds(4f);
        //Obtención de una ubicación aleatoria dentro del campo de juego
        UnityEngine.Vector3 position = new UnityEngine.Vector3(Random.Range(-12, 12), 0.2f, 22);
        CreateEnemy(enemyPrefabs[1], position);
        //Se espera medio segundo para generar la pareja
        yield return new WaitForSeconds(0.5f);
        CreateEnemy(enemyPrefabs[1], position);
        //Creación de Parchis nuevamente
        StartCoroutine(SpawnDamas());
    }
}
