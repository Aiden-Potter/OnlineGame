using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PanelMgr.instance.OpenPanel<TitlePanel>("");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
