using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 100f;
    public GameObject explode;
    public float maxLifeTime = 2f;
    public float instantiateTime = 0f;
    public GameObject attackTank;
    public AudioClip explodeClip;
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
        GameObject explodeObj = Instantiate(explode, transform.position,Quaternion.identity);
        AudioSource explodeAudio = explodeObj.AddComponent<AudioSource>();
        explodeAudio.spatialBlend = 1;
        explodeAudio.PlayOneShot(explodeClip,2.0f);

        if(collision.gameObject.tag == "Tank")
        {
            Tank tank = collision.gameObject.GetComponent<Tank>();
            if(tank!= null)
            {
                float att = GetAtt();
                tank.BeAttacked(att,attackTank);
            }
        }


        Destroy(gameObject);
    }
    float GetAtt()
    {
        float att = 100 - (Time.time - instantiateTime) * 40;
        if (att < 10)
            att = 10;
       
        return att;
    }
}
