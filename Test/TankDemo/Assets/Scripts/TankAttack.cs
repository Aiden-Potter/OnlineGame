using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAttack : MonoBehaviour
{
    public GameObject bullet;
    public Transform gun;
    public float lastShootTime = 0f;
    public float shootInterval = 0.5f;

    public void Shoot()
    {
        if (Time.time - lastShootTime < shootInterval) return;
        if (bullet == null) return;

        Vector3 pos = gun.position + gun.forward * 5;

        Instantiate(bullet, pos, gun.rotation);
        lastShootTime = Time.time;
    }
}

