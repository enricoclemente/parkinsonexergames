using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMovement
{
    float sensitivity = 4f;

    public float GetVerticalAxis(nuitrack.Skeleton skeleton)
    {
        Vector3 upBodyDirection = (JointPosition(skeleton, nuitrack.JointType.Head) - JointPosition(skeleton, nuitrack.JointType.Waist)).normalized; // direzione tra testa e bacino:
        // quantifichiamo lo spostamento di z 
        float verticalAxis = Mathf.Clamp(Vector3.Dot(upBodyDirection, Vector3.back) * sensitivity, -1, 1);   // restringe il valore tra min e max (-1,1)
        return verticalAxis;
    }

    private Vector3 JointPosition(nuitrack.Skeleton skeleton, nuitrack.JointType joint)
    {
        return skeleton.GetJoint(joint).ToVector3() * 0.001f;
    }
}
