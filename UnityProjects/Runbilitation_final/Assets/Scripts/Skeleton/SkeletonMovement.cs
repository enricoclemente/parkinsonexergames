using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkeletonMovement : MonoBehaviour
{
    [SerializeField] public float sensitivityInclination = 10f;   // sensibilità per l'inclinazione del torso sia side che front
    [SerializeField] public float sideInclinationThreshold = 0.125f;   // soglia per cui rilevare l'inclinazione laterale
    [SerializeField] public float frontInclinationThreshold = 0.3f;    // soglia per cui rilevare inclinazione frontale
    [SerializeField] public float shouldersThreshold = 20f;         // soglia per rilevare la rotazione delle spalle
    [SerializeField] public float armThreshold = 0.05f;         // soglia per rilevare il sollevamento delle braccia

    Quaternion defaultRotationXY = Quaternion.LookRotation(Vector3.left, Vector3.up);   // quaternione di default: rotazione di riferimento data dagli assi x e y

    // variabili utilizzate per comandare il personaggio
    private float sideInclination = 0;
    private bool frontInclination = false;
    private bool bothArmUp = false;
    private Quaternion deltaShoulderRotation = new Quaternion(0,0,0,0);
   
    
    void Update()
    {
        if (CurrentUserTracker.CurrentUser != 0)   // se rileva lo skeleton
        {
            nuitrack.Skeleton skeleton = CurrentUserTracker.CurrentSkeleton;   // prendo il riferimento dello scheletro rilevato da nuitrack
            CalculateBodyInclination(skeleton);
            CalculateShouldersRotation(skeleton);
            CalculateArmsInclination(skeleton);
        } 
        else
        {
            // resetto tutte le variabili
            ResetAll();
        }
    }

    private void CalculateBodyInclination(nuitrack.Skeleton skeleton)  // determina l'inclinazione laterale e frontale del personaggio
    {
        Vector3 upBodyDirection = (JointPosition(skeleton, nuitrack.JointType.Head)    // vettore direzione tra testa e bacino:
            - JointPosition(skeleton, nuitrack.JointType.Waist)).normalized;             

        if (Mathf.Abs(upBodyDirection.x) > sideInclinationThreshold)   // se la componente x è maggiore della soglia di inclinazione laterale
        {
            sideInclination = Mathf.Clamp(Vector3.Dot(upBodyDirection, Vector3.left) * sensitivityInclination, -1, 1);  // rimappa il valore tra min e max (-1,1)
        }
        else
        {
            sideInclination = 0f;
        }

        if (upBodyDirection.z < -frontInclinationThreshold)   // se la componente z è minore della soglia di inclinazione frontale
        {
            frontInclination = true;            
        }
        else
        {
            frontInclination = false;
        }
    }

    private void CalculateShouldersRotation(nuitrack.Skeleton skeleton)
    {
        Vector3 shouldersDirection = (JointPosition(skeleton, nuitrack.JointType.RightShoulder)
            - JointPosition(skeleton, nuitrack.JointType.LeftShoulder));   // vettore direzione tra spalla dx e sx

        shouldersDirection.y = 0;     // azzera possibile rotazione verticale indesiderata 
        Quaternion shoulderRotation = Quaternion.LookRotation(shouldersDirection, Vector3.up);   // misura rotazione tra spalle e asse y
        
        if (Quaternion.Angle(shoulderRotation, defaultRotationXY) > shouldersThreshold)   // se l'angolo tra rotazione precedente e quella di riferimento è maggiore di un soglia
        {
            deltaShoulderRotation = Quaternion.Inverse(defaultRotationXY) * shoulderRotation;
        }

        else
        {
            deltaShoulderRotation = new Quaternion(0,0,0,0);  
        }
    }

    private void CalculateArmsInclination(nuitrack.Skeleton skeleton)
    {
        Vector3 rightArmDirection = JointPosition(skeleton, nuitrack.JointType.RightHand) - JointPosition(skeleton, nuitrack.JointType.RightShoulder); // vettore direzione tra mano dx e spalla dx
        Vector3 leftArmDirection = JointPosition(skeleton, nuitrack.JointType.LeftHand) - JointPosition(skeleton, nuitrack.JointType.LeftShoulder); // vettore direzione tra mano sx e spalla sx

        if (rightArmDirection.y > armThreshold && leftArmDirection.y > armThreshold)  // vedo che la componente y della direzione del braccio sia maggiore della soglia: 
        {
            bothArmUp = true;            
        }
        else
        {            
            bothArmUp = false;
        }
    }

    private Vector3 JointPosition(nuitrack.Skeleton skeleton, nuitrack.JointType joint) // rimappare posizione dei joints in sistema di coordinate unity
    {
        return skeleton.GetJoint(joint).ToVector3() * 0.001f; 
    }

    public float GetSideInclination()  // ritorna l'inclinazione laterale del personaggio
    {
        return sideInclination;
    }

    public bool GetFrontInclination()  // ritorna l'inclinazione in avanti del personaggio
    {
        return frontInclination;
    }

    public Quaternion GetDeltaShoulderRotation()
    {
        return deltaShoulderRotation;  // ritorna rotazione del personaggio
    }

    public bool GetBothArmUp()
    {
        return bothArmUp;
    }

    private void ResetAll()
    {
        sideInclination = 0;
        frontInclination = false;
        bothArmUp = false;
        deltaShoulderRotation = new Quaternion(0,0,0,0);
    }
}
