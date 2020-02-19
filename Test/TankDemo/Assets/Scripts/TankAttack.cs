using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAttack : MonoBehaviour
{
    public GameObject bullet;
    public Transform gun;
    public float lastShootTime = 0f;
    public float shootInterval = 0.5f;
    public Tank tank;
    public void Awake()
    {
        tank = GetComponent<Tank>();
    }
    public void Shoot()
    {
        if (Time.time - lastShootTime < shootInterval) return;
        if (bullet == null) return;

        Vector3 pos = gun.position + gun.forward * 5;

        GameObject bulletObj = Instantiate(bullet, pos, gun.rotation);
        Bullet bulletCmp = bulletObj.GetComponent<Bullet>();
        if(bulletCmp !=null)
        {
            bulletCmp.attackTank = this.gameObject;
        }
        lastShootTime = Time.time;
        tank.shootAudio.PlayOneShot(tank.shootClip);
        //sound
        
    }
}

