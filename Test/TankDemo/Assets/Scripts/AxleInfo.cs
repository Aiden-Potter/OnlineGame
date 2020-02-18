using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AXleInfo
{
    
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    //两个车轮碰撞器
    public bool motor;//是否有马力传达，控制前驱后驱
    public bool steering;//前轮转向
}
