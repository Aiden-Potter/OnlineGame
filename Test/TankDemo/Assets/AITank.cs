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
    void Start()
    {
        
    }

    void Update()
    {
        if (tank.ctrlType != CtrlType.computer)
            return;
        TargetUpdate();//每隔几秒检查一下跳转状态

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

    }
    void AttackUpdate()
    {

    }
    void PatrolStart()
    {

    }
    void PatrolUpdate()
    {

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
        }
        else if(Vector3.Distance(pos,targetPos)>sightDistance)
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
            if (tank==null)
                continue;
            if (targets[i] == gameObject)
                continue;
            if (tank.ctrlType == CtrlType.none)
                continue;

            Vector3 pos = transform.position;
            Vector3 targetPos = targets[i].transform.position;
            if (Vector3.Distance(pos, targetPos) > sightDistance)
                continue;
            //并没有实现朝着生命值最小的那个坦克运动
            if (minHp>tank.hp)
            {
                target = tank.gameObject;
            }
        }

        if (target!=null)
        {
            Debug.Log("Get Target :" + target.name);
        }
    }

    public void OnAttacked(GameObject attackTank)
    {
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
}
