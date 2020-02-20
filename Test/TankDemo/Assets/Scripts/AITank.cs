using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITank : MonoBehaviour
{
    public enum Status
    {
        Patrol,
        Attack,
    }
    public Tank tank;
    private Status status = Status.Patrol;//默认巡逻
    private GameObject target;
    private float sightDistance = 30f;
    private float lastSearchTargetTime = 0;
    private float searchTargetInterval = 3f;

    private Path path = new Path();

    private float lastUpdateWaypointInterval = float.MinValue;
    private float updateWaypointInterval = 5;
    
    void InitWayPoint()
    {
        GameObject obj = GameObject.Find("WaypointContainer");
        if (obj&&obj.transform.GetChild(0)!=null)
        {
            Vector3 targetPos = obj.transform.GetChild(0).position;

            path.InitByNavMeshPath(transform.position, targetPos);

        }
    }
    void Start()
    {
        InitWayPoint();
    }

    void Update()
    {
        if (tank.ctrlType != CtrlType.computer)
            return;
        TargetUpdate();//每隔几秒检查一下跳转状态

        if (path.IsReach(transform))
        {
            path.NextWayPoint();
        }
        Debug.Log(status);
        if (status == Status.Patrol)
            PatrolUpdate();
        else if (status == Status.Attack)
            AttackUpdate();
    }
    public void ChangeStatus(Status status)
    {
        if (status == Status.Attack)
            AttackStart();
        else if (status == Status.Patrol)
            PatrolStart();
    }

    void AttackStart()
    {
        Vector3 targetPos = target.transform.position;
        path.InitByNavMeshPath(transform.position, targetPos);

    }
    void AttackUpdate()
    {
        if (target == null)
        {
            ChangeStatus(Status.Patrol);   
        }
        float interval = Time.time - lastUpdateWaypointInterval;
        if (interval < updateWaypointInterval)
        {
            return;
        }
        lastUpdateWaypointInterval = Time.time;
        Vector3 targetPos = target.transform.position;
        path.InitByNavMeshPath(transform.position, targetPos);
    }
    void PatrolStart()
    {
        Debug.Log("恢复巡逻");
    }
    void PatrolUpdate()
    {
        if (target != null)
        {
            ChangeStatus(Status.Attack);
        }
        float interval = Time.time - lastUpdateWaypointInterval;
        if (interval < updateWaypointInterval)
        {
            return;
        }
        lastUpdateWaypointInterval = Time.time;

        if (path.waypoints == null || path.isFinish)
        {
            GameObject obj = GameObject.Find("WaypointContainer");
            //免得变量名重名加个{}
            {
                int count = obj.transform.childCount;
                if (count == 0)
                {
                    return;
                }
                int index = Random.Range(0, count);
                Vector3 targetPos = obj.transform.GetChild(index).position;
                path.InitByNavMeshPath(transform.position, targetPos);
            }   
        }
    }

    void TargetUpdate()
    {
        float interval = Time.time - lastSearchTargetTime;
        if (interval < searchTargetInterval)
            return;
        lastSearchTargetTime = Time.time;

        if (target != null)
            HasTarget();
        else
            NoTarget();

    }
    void HasTarget()
    {
        Tank targetTank = target.GetComponent<Tank>();
        Vector3 pos = transform.position;
        Vector3 targetPos = target.transform.position;

        if (targetTank.ctrlType == CtrlType.none)
        {
            //target already died
            Debug.Log("Target Die,Lost Target!");
            target = null;
            path.isFinish = true;
        }
        else if (Vector3.Distance(pos, targetPos) > sightDistance)
        {
            //target escape
            Debug.Log("The distance so far,Lose Target");
            target = null;
        }
    }
    void NoTarget()
    {
        float minHp = float.MaxValue;
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Tank");
        for (int i = 0; i < targets.Length; i++)
        {
            Tank tank = targets[i].GetComponent<Tank>();
            if (tank == null)
                continue;
            if (targets[i] == gameObject)
                continue;
            if (Battle.Instance.IsSameCamp(gameObject, targets[i]))
                continue;
            if (tank.ctrlType == CtrlType.none)
                continue;

            Vector3 pos = transform.position;
            Vector3 targetPos = targets[i].transform.position;
            if (Vector3.Distance(pos, targetPos) > sightDistance)
                continue;
            //并没有实现朝着生命值最小的那个坦克运动
            if (minHp > tank.hp)
            {
                target = tank.gameObject;
            }
        }

        if (target != null)
        {
            Debug.Log("Get Target :" + target.name);
        }
    }

    public void OnAttacked(GameObject attackTank)
    {
        if (Battle.Instance.IsSameCamp(gameObject,attackTank))
        {
            return;
        }
        target = attackTank;
    }

    public Vector3 GetTurrentTarget()
    {
        if (target == null)
        {
            float y = transform.eulerAngles.y;
            Vector3 rot = new Vector3(0, y, 0);
            return rot;
        }
        else
        {
            Vector3 pos = transform.position;
            Vector3 targetPos = target.transform.position;
            Vector3 vec = targetPos - pos;
            return Quaternion.LookRotation(vec).eulerAngles;
        }
    }

    public bool IsShoot()
    {
        if (target == null)
        {
            return false;
        }
        float turrentRoll = tank.turret.eulerAngles.y;
        float angle = turrentRoll - GetTurrentTarget().y;
        if (angle < 0) angle += 360;

        if (angle < 30 || angle > 330)
            return true;
        else
            return false;
    }

    //获取旋转角度
    public float GetSteering()
    {
        if (tank == null)
        {
            return 0;
        }
        Vector3 itp = transform.InverseTransformPoint(path.waypoint);//world space point to local space point
        if (itp.x > path.deviation / 5)
        {
            return tank.maxSteeringAngle;
        }
        else if (itp.x < -path.deviation / 5)
        {
            return -tank.maxSteeringAngle;
        }
        else return 0;
    }

    public float GetMotor()
    {
        if (tank == null)
        {
            return 0; 
        }
        Vector3 itp = transform.InverseTransformPoint(path.waypoint);
        float x = itp.x;
        float z = itp.z;
        float r = 10;

        if (z<0&&Mathf.Abs(x)<-z&&Mathf.Abs(x)<r)
        {
            return -tank.maxMotorTorque;
        }
        else if(Vector3.Distance(path.waypoint,transform.position)>20)
        {
            return tank.maxMotorTorque;
        }
        else
        {
            return tank.maxMotorTorque / 2;
        }
    }
    public float GetBrake()
    {
        if (tank == null)
        {
            return 0;
        }
        Debug.Log(path.isFinish);
        if (path.isFinish)
        {
            return tank.maxBrakeTorque;
        }
        else
        {
            return 0;
        }
    }
    private void OnDrawGizmos()
    {
        path.DrawWayPoints();
    }

    
}
