using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    public float speed=3.0f;
    public float rotateSpeed=20.0f;
    public Transform turret;
    public Transform gun;
    public float turretRotTarget;
    public float turretRotSpeed = 0.5f;

    public float gunRotTarget;
    public float gunRotSpeed=0.5f;

    private float maxRoll = 4.0f;
    private float minRoll =-10.0f;
    void Start()
    {
        turret = transform.Find("turret");
        gun = turret.Find("gun");
    }

    void Update()
    {
        move();

        turretRotTarget = Camera.main.transform.eulerAngles.y;
        gunRotTarget = Camera.main.transform.eulerAngles.x;
       // TurretRotation();
        GunRotation();
    }

    void move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        transform.Rotate(0, x * rotateSpeed * Time.deltaTime, 0);
        transform.position += y * speed * transform.forward * Time.deltaTime;
    }

    void TurretRotation()
    {
        if (turret == null)
            return;
        if (Camera.main == null)
            return;
        float angle = turret.eulerAngles.y - turretRotTarget;
        if (angle < 0)
            angle += 360;
        if (angle > 180 && angle < 360-turretRotSpeed)
            turret.Rotate(0, turretRotSpeed, 0);
        else if (angle > turretRotSpeed&&angle < 180 )
            turret.Rotate(0, -turretRotSpeed, 0);
    }

    void GunRotation()
    {
        if (gun == null)
            return;
        if (Camera.main == null)
            return;
        Vector3 worldEuler = gun.eulerAngles;
        Vector3 localEuler = gun.localEulerAngles;
        worldEuler.x = gunRotTarget;
        gun.eulerAngles = worldEuler;//用世界坐标系去计算炮管转到的位置

        Vector3 euler = gun.localEulerAngles;
        if (euler.x > 180)//世界坐标系下euler怎么转是正的
            euler.x -= 360;
        if (euler.x > maxRoll)
            euler.x = maxRoll;
        if (euler.x < minRoll)
            euler.x = minRoll;
        gun.localEulerAngles = new Vector3(euler.x, localEuler.y, localEuler.z);
    }
}
