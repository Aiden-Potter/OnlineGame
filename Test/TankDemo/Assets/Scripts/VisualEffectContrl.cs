using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;
public class VisualEffectContrl : MonoBehaviour
{
    // Start is called before the first frame update
    public VisualEffect ve;
    public float maxLifeTime = 5.0f;
    public float destoryTime = 7.0f;
    private float instantiateTime;
    void Start()
    {
        ve.SendEvent("OnPlay");
        instantiateTime = Time.time;

;   }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - instantiateTime >maxLifeTime)
        {
            ve.SendEvent("OnStop");
        }
        if (Time.time - instantiateTime > destoryTime)
        {
            Destroy(gameObject);
        }
    }
}
