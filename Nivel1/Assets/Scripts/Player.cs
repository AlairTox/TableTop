using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] float moveSpeed;
    [SerializeField] float minX, minY, minZ, maxX, maxY, maxZ, padding;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        float deltaX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        //float deltaY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        float deltaZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        //float deltaZ = Input.GetAxis("AxisZ") * moveSpeed * Time.deltaTime;
        
        float newPosX = Mathf.Clamp(transform.position.x + deltaX, minX + padding, maxX - padding);
        //float newPosY = Mathf.Clamp(transform.position.y + deltaY, minY + padding, maxY - padding);
        float newPosZ = Mathf.Clamp(transform.position.z + deltaZ, minZ + padding, maxZ - padding);
        
        transform.position = new Vector3(newPosX, transform.position.y, newPosZ);        
    }
}

