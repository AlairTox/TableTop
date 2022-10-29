using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    // Start is called before the first frame update
    public void DestroyOnComplete()
    {
        Destroy(gameObject);
    }

}
