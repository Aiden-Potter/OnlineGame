using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float distance = 15.0f;
    [Header("Angle")]
    public float rot = 90f * Mathf.PI * 2 / 360;
    public float roll = 30f * Mathf.PI * 2 / 360;
    public float rotSpeed=0.2f;
    public float rollSpeed = 0.3f;
    private GameObject target;
    private float maxRoll = 70.0f * Mathf.PI / 180;
    private float minRoll = -20.0f * Mathf.PI / 180;

    public float maxDistance = 22.0f;
    public float minDistance = 5.0f;

    public float zoomSpeed = 0.2f;
    void Start()
    {
        SetTarget(GameObject.Find("Tank"));
    }


    void LateUpdate()
    {
        if (target == null)
            return;
        if (Camera.main == null)
            return;
        Zoom();
        UpdateAngle();
        CameraMove();
    }

    void SetTarget(GameObject tar)
    {
        if (tar != null)
            if (tar.transform.Find("cameraPoint") != null)
                target = tar.transform.Find("cameraPoint").gameObject;
            else target = tar;
        else return;

    }
    void CameraMove()
    {
        Vector3 targetPos = target.transform.position;
        float xz = distance * Mathf.Cos(roll);
        Vector3 cameraPos = new Vector3(targetPos.x + xz * Mathf.Cos(rot)
            , targetPos.y + distance * Mathf.Sin(roll),
            targetPos.z + xz * Mathf.Sin(rot));
        Camera.main.transform.position = cameraPos;
        Camera.main.transform.LookAt(target.transform);
    }
    void UpdateAngle()
    {
        float w= Input.GetAxis("Mouse Y") * rollSpeed;
        roll -= w;
        if (roll > maxRoll)
            roll = maxRoll;
        if (roll < minRoll)
            roll = minRoll;
        float p = Input.GetAxis("Mouse X") * rotSpeed;
        rot -= p;
    }

    void Zoom()
    {
        float d = Input.GetAxis("Mouse ScrollWheel");
        if (d > 0&&distance > minDistance)
            distance -= zoomSpeed;
        else if (d < 0&&distance < maxDistance) 
            distance += zoomSpeed;

    }
}
