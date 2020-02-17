using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 100f;
    public GameObject explode;
    public float maxLifeTime = 2f;
    public float instantiateTime = 0f;

    void Start()
    {
        instantiateTime = Time.time;    
    }


    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        if(Time.time-instantiateTime >maxLifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Boom!!");
        Instantiate(explode, transform.position,Quaternion.identity);

        if(collision.gameObject.tag == "Tank")
        {
            TankControllerMotor tank = collision.gameObject.GetComponent<TankControllerMotor>();
            if(tank!= null)
            {
                float att = GetAtt();
                tank.BeAttacked(att);
            }
        }


        Destroy(gameObject);
    }
    float GetAtt()
    {
        float att = 100 - (Time.time - instantiateTime) * 40;
        if (att < 10)
            att = 10;
        Debug.Log(att);
        return att;
    }
}
