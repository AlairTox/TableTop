using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sistemaVidas : MonoBehaviour
{
    [SerializeField] GameObject[] hearts;
    private int life;
    void Start(){
        life = hearts.Length;
    }

    public void changeLifes(){
        life--;
        Destroy(hearts[life].gameObject);
    }

}
