using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CtrlType
{
    none,
    player,
    computer
}

public class TankControllerMotor : MonoBehaviour
{
    public List<AXleInfo> axleInfos;
    public CtrlType ctrlType = CtrlType.player;
    private float motor = 0;
    public float maxMotorTorque;

    private float brakeTorque = 0;
    public float maxBrakeTorque = 100;

    private float steering = 0;
    public float maxSteeringAngle;
    public Transform turret;
    public Transform gun;
    public Transform g;
    public Transform wheelObj;
    public Transform track;
    public List<Transform> wheels;
    public Rigidbody rigidbody;
    public AudioSource motorAudioSource;
    public AudioClip motorClip;
    public float turretRotTarget;
    public float turretRotSpeed = 0.5f;

    public float gunRotTarget;
    public float gunRotSpeed = 0.5f;

    public TankAttack tankAttack;

    public float hp = 100f;
    private float maxHp = 100f;
    public GameObject destoryEffect;
    private float maxRoll = 5.0f;
    private float minRoll = -10.0f;
    void Start()
    {
        turret = transform.Find("turret");
        gun = turret.Find("gun");
        wheelObj = transform.Find("Wheels");
        track = transform.Find("Track");
        wheels = new List<Transform>(wheelObj.GetComponentsInChildren<Transform>());
        wheels.RemoveAt(0);
        rigidbody = gameObject.GetComponent<Rigidbody>();
        motorAudioSource = gameObject.AddComponent<AudioSource>();
        motorAudioSource.spatialBlend = 1;//播放3d的音效，范围0~·，1为3d，0为2d，混合效果
    }

    void Update()
    {
        if (ctrlType != CtrlType.player) return;
        PlayerCtrl();
        Accerlerate();
        MotorSound();
        TurretRotation();
        GunRotation();
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
        if (angle > 180 && angle < 360 - turretRotSpeed)
            turret.Rotate(0, turretRotSpeed, 0);
        else if (angle > turretRotSpeed && angle < 180)
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
        if (Input.GetKey(KeyCode.Q))
            worldEuler.x += gunRotSpeed;
        if (Input.GetKey(KeyCode.E))
            worldEuler.x -= gunRotSpeed;//按键冲突？
        gun.eulerAngles = worldEuler;
        Vector3 euler = gun.localEulerAngles;
        if (euler.x > 180)//localEulerAngles一直减过了0会变成360
            euler.x -= 360;
        if (euler.x > maxRoll)
            euler.x = maxRoll;
        if (euler.x < minRoll)
            euler.x = minRoll;
        gun.localEulerAngles = new Vector3(euler.x, localEuler.y, localEuler.z);

    }
    public void PlayerCtrl()
    {
        

        if (Input.GetMouseButtonDown(0))
        {
            tankAttack.Shoot();
           
        }

        motor = maxMotorTorque * Input.GetAxis("Vertical");
        steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        brakeTorque = 0;
        foreach (var item in axleInfos)
        {
            if (item.leftWheel.rpm > 5 && motor < 0)
                brakeTorque = maxBrakeTorque;
            else if (item.leftWheel.rpm < -5 && motor > 0)
                brakeTorque = maxBrakeTorque;
        }

        turretRotTarget = Camera.main.transform.eulerAngles.y;
    }
    public void Accerlerate()
    {
        foreach (var item in axleInfos)
        {
            if (item.steering)
            {
                item.leftWheel.steerAngle = steering;
                item.rightWheel.steerAngle = steering;
            }
            if (item.motor)
            {
                item.leftWheel.motorTorque = motor;
                item.rightWheel.motorTorque = motor;
            }
            if (true)
            {
                item.leftWheel.brakeTorque = brakeTorque;
                item.rightWheel.brakeTorque = brakeTorque;
            }
        }
        rigidbody.centerOfMass = g.localPosition;
        if (axleInfos[1] != null)//这一步把碰撞器的旋转状态赋给每一个轮子
        {
            wheelsRotation(axleInfos[1].leftWheel);
            TrackMove();
        }
    }
    public void wheelsRotation(WheelCollider collider)
    {
        if (wheels == null) return;
        Vector3 pos;
        Quaternion rotation;

        collider.GetWorldPose(out pos, out rotation);
        foreach (var item in wheels)
        {
            item.rotation = rotation;
        }
    }

    public void TrackMove()
    {
        if (track == null) return;
        float offset = 0;
        if (wheels[0] != null)
            offset = wheels[0].localEulerAngles.x / 90.0f;
        track.GetChild(0).GetComponent<MeshRenderer>().material.mainTextureOffset = new Vector2(0, offset);
    }
    void MotorSound()
    {
        if(motor!=0&&!motorAudioSource.isPlaying)
        {
            motorAudioSource.loop = true;
            motorAudioSource.clip = motorClip;
            motorAudioSource.Play();
        }else if(motor==0)
        {
            motorAudioSource.Pause();
        }
    }

    public void BeAttacked(float att)
    {
        
        if (hp <= 0)
            return;//已经死亡
        Debug.Log("damage:" + att);
        if (hp>0)
        {
            hp -= att;
        }
        if (hp<=0)
        {
            GameObject destoryOgbj = GameObject.Instantiate(destoryEffect,transform);
            destoryOgbj.transform.localPosition = Vector3.zero;
            ctrlType = CtrlType.none;
        }
    }
}
