using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class modifyPosition : MonoBehaviour
{
    RectTransform position;
    Vector3 newPosition = new Vector3(250, 152, 0);
    // Start is called before the first frame update
    void Start()
    {
        position = GetComponent<RectTransform>();
        position.localPosition = newPosition;
    }
}
