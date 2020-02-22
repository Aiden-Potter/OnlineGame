using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformTest : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform cube1;
    public Transform cube2;

    public Vector3 pos1;
    public Vector3 pos2;
    public Vector3 pos3;
    public Vector3 pos4;
    public Vector3 vec1;
    public Vector3 vec2;
    public Vector3 vec3;
    public Vector3 vec4;

    public Vector3 dir1;
    public Vector3 dir2;
    public Vector3 dir3;
    public Vector3 dir4;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pos1 = transform.TransformPoint(cube1.position);
        pos2 = transform.TransformPoint(cube2.position);
        pos3 = cube2.TransformPoint(cube1.position);
        pos4 = transform.InverseTransformPoint(cube1.position);

        vec1 = transform.TransformVector(cube1.position);
        vec2 = transform.TransformVector(cube2.position);
        vec3 = cube2.TransformVector(cube1.position);
        vec4 = transform.InverseTransformVector(cube1.position);

        dir1 = transform.TransformDirection(cube1.position);
        dir2 = transform.TransformDirection(cube2.position);
        dir3 = cube2.TransformDirection(cube1.position);
        dir4 = transform.InverseTransformDirection(cube1.position);


    }
}
