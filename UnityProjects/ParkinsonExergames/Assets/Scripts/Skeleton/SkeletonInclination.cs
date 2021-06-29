using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkeletonRender : MonoBehaviour
{
    public nuitrack.JointType[] joints;
    public GameObject sphere;

    private GameObject[] createdJoints;

    // Start is called before the first frame update
    void Start()
    {
        createdJoints = new GameObject[joints.Length];
        for (int i = 0; i < joints.Length; i++)
        {
            createdJoints[i] = Instantiate(sphere);
            createdJoints[i].transform.SetParent(this.transform);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (CurrentUserTracker.CurrentUser != 0)     // se rileva lo skeleton
        {
            nuitrack.Skeleton skeleton = CurrentUserTracker.CurrentSkeleton;    // prende lo skeleton

            for (int i = 0; i < joints.Length; i++)
            {
                nuitrack.Joint joint = skeleton.GetJoint(joints[i]);
                Vector3 newPos = joint.ToVector3() * 0.01f;
                createdJoints[i].transform.localPosition = newPos;
            }
        }
    }

}
