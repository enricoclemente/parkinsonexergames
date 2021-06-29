using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkeletonMovement : MonoBehaviour
{
    [SerializeField] public float sensitivityInclination = 2f;  // sensibilità per l'inclinazione del torso sia side che back
    [SerializeField] public float inclinationThreshold = 0.1f;  
    [SerializeField] public float shouldersThreshold = 10f;   
    [SerializeField] public float armThreshold = 0.1f;
    [SerializeField] public int armPersistency = 10;            // una specie di timer per gestire i due comandi differenti delle braccia
    Quaternion defaultRotationXY =
        Quaternion.LookRotation(Vector3.left, Vector3.up);      // quaternione di default: rotazione standard

    // variabili utilizzate per comandare il personaggio
    private float sideInclination = 0;
    private bool backInclination = false;
    private bool rightArmUp = false;
    private bool bothArmUp = false;
    private Quaternion deltaShoulderRotation;
   
    
    void Update()
    {
        if (CurrentUserTracker.CurrentUser != 0)     // se rileva lo skeleton
        {
            nuitrack.Skeleton skeleton = CurrentUserTracker.CurrentSkeleton;
            CalculateBodyInclination(skeleton);
            CalculateShouldersRotation(skeleton);
            CalculateArmsInclination(skeleton);
        } else
        {
            // resetto tutte le variabili
            ResetAll();
        }
    }

    /*
    public void Calculate()
    {
        if (CurrentUserTracker.CurrentUser != 0)     // se rileva lo skeleton
        {
            nuitrack.Skeleton skeleton = CurrentUserTracker.CurrentSkeleton;
            CalculateBodyInclination(skeleton);
            CalculateShouldersRotation(skeleton);
            CalculateArmsInclination(skeleton);
        }
    } */


    private void CalculateBodyInclination(nuitrack.Skeleton skeleton)  // determina l'inclinazione laterale e all'indietro del personaggio
    {
        Vector3 upBodyDirection = (JointPosition(skeleton, nuitrack.JointType.Head)
            - JointPosition(skeleton, nuitrack.JointType.Waist)).normalized;        // direzione tra testa e bacino:
                                                                                    // quantifichiamo lo spostamento di z 
        //sideInclination = Mathf.Clamp(Vector3.Dot(upBodyDirection, Vector3.left)
        //    * sensitivityInclination, -1, 1);                                       // restringe il valore tra min e max (-1,1)

        if(Mathf.Abs(upBodyDirection.x) > inclinationThreshold)
        {
            sideInclination = Mathf.Clamp(Vector3.Dot(upBodyDirection, Vector3.left)
                * sensitivityInclination, -1, 1);
        }

        if (upBodyDirection.z > inclinationThreshold)
        {
            backInclination = true;
        }
        else
        {
            backInclination = false;
        }
    }

    private void CalculateShouldersRotation(nuitrack.Skeleton skeleton)
    {
        Vector3 shouldersDirection = (JointPosition(skeleton, nuitrack.JointType.RightShoulder) - JointPosition(skeleton, nuitrack.JointType.LeftShoulder));
        shouldersDirection.y = 0;  // assera possibile inclinazione 
        Quaternion shoulderRotation = Quaternion.LookRotation(shouldersDirection, Vector3.up);   // misura inclinazione tra spalle e asse y
        if (Quaternion.Angle(shoulderRotation, defaultRotationXY) > shouldersThreshold)
        {
            deltaShoulderRotation = Quaternion.Inverse(defaultRotationXY) * shoulderRotation;
        }
    }

    private void CalculateArmsInclination(nuitrack.Skeleton skeleton)
    {
        Vector3 rightArmDirection = JointPosition(skeleton, nuitrack.JointType.RightHand) - JointPosition(skeleton, nuitrack.JointType.RightShoulder);
        Vector3 leftArmDirection = JointPosition(skeleton, nuitrack.JointType.LeftHand) - JointPosition(skeleton, nuitrack.JointType.LeftShoulder);

        // vedo che la componente y della direzione del braccio sia maggiore del threshold
        if (rightArmDirection.y > armThreshold)
        {
            if (armPersistency > 0)
            {
                armPersistency--;
            }
            else
            {
                if (leftArmDirection.y > armThreshold)
                {
                    bothArmUp = true;
                }
                else
                {
                    rightArmUp = true;
                }
                armPersistency = 10;
            }
        }
        else
        {
            rightArmUp = false;
            bothArmUp = false;
        }
    }

    private Vector3 JointPosition(nuitrack.Skeleton skeleton, nuitrack.JointType joint)
    {
        return skeleton.GetJoint(joint).ToVector3() * 0.001f;
    }

    public float GetSideInclination()  // ritorna l'inclinazione laterale del personaggio
    {
        return sideInclination;
    }

    public bool GetBackInclination()  // ritorna l'inclinazione all'indietro del personaggio
    {
        return backInclination;
    }

    public Quaternion GetDeltaShoulderRotation()
    {
        return deltaShoulderRotation;  // ritorna rotazione del personaggio
    }

    public bool GetBothArmUp()
    {
        return bothArmUp;
    }
    public bool GetRightArmUp()
    {
        return rightArmUp;
    }

    private void ResetAll()
    {
        sideInclination = 0;
        backInclination = false;
        rightArmUp = false;
        bothArmUp = false;
        deltaShoulderRotation = defaultRotationXY;
    }
}



