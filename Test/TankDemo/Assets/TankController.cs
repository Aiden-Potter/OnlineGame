using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    public float speed=3.0f;
    public float rotateSpeed=20.0f;
    void Start()
    {
        
    }

    void Update()
    {
        move();
    }

    void move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        transform.Rotate(0, x * rotateSpeed * Time.deltaTime, 0);
        transform.position += y * speed * transform.forward * Time.deltaTime;
    }
}
