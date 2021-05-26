using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyBodyParts : MonoBehaviour
{
    public float breakDistance;
    private SpringJoint2D springJoint;
    private HingeJoint2D hingeJoint;
    // Start is called before the first frame update
    void Start()
    {
        hingeJoint = GetComponent<HingeJoint2D>();
        springJoint = GetComponent<SpringJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (springJoint.distance >= breakDistance)
        {
            hingeJoint.enabled = false;
            springJoint.enabled = false;
        }
    }
}
