using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] float moveSpeed;
    [SerializeField] float minX, minY, minZ, maxX, maxY, maxZ, padding;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        float deltaX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime * 5;
        float deltaZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime * 5;
        
        float newPosX = Mathf.Clamp(transform.position.x + deltaX, minX + padding, maxX - padding);
        float newPosZ = Mathf.Clamp(transform.position.z + deltaZ, minZ + padding, maxZ - padding);
        
        rb.MovePosition(new Vector3(newPosX, transform.position.y, newPosZ));        
    }
}

