using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    HingeJoint2D joint;

    void Awake()
    {
        joint = GetComponent<HingeJoint2D>();    
    }

    // Update is called once per frame
    void Update()
    {
        print(joint.jointSpeed);
    }
}
