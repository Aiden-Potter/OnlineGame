using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;
public class VisualEffectContrl : MonoBehaviour
{
    // Start is called before the first frame update
    public VisualEffect ve;
    void Start()
    {
        ve.SendEvent("OnStop");
;   }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
           {
            ve.SendEvent("OnPlay");
        }

    }
}
