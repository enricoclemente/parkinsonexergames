using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public Camera characterCamera;
    Vector3 cameraOffset;
    float characterGroundHeight;

    SkeletonMovement skeletonMovement;

    [SerializeField] public float speedMovement = 5f;
    [SerializeField] public float speedInclination = 2f;
    [SerializeField] public float speedRotation = 30f;
    [SerializeField] public float jumpForce = 8f;


    private bool slideDownPast = false;
    private bool jumpPast = false;
    

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = characterCamera.transform.position - this.transform.position;    // vettore differenza tra camera e personaggio
        characterGroundHeight = transform.position.y;                                   // salvo altezza iniziale personaggio
        animator = gameObject.GetComponent<Animator>();
        skeletonMovement = GameObject.FindGameObjectWithTag("UserMovements").GetComponent<SkeletonMovement>();  // prendo il riferimento della classa SkeletonMovement istanziata sul game object Skeleton
        Debug.Assert(skeletonMovement != null);    // check if the reference is valid
    }

    void Update()
    {        
        float sideInclination = skeletonMovement.GetSideInclination();   // left or right movement
              
        bool slideDown = skeletonMovement.GetFrontInclination(); // slide the character down

        if (slideDown == true)   // se mi devo inclinare 
        {
            if (slideDown != slideDownPast)    // e prima non lo stavo facendo
            {
                animator.SetBool("isSlide", true);       // animazione inclinazione
                transform.Translate(Vector3.forward * speedMovement);        // spinta in avanti
                slideDownPast = true;
            }
            else
            {
                animator.SetBool("isSlide", false);
            }
        }
        else if (slideDown == false)  // se non mi sto piu' inclinando aggiorno
        {
            slideDownPast = false;    
        }

        // Rotation:
        Quaternion shoulderRotation = skeletonMovement.GetDeltaShoulderRotation();
        transform.rotation = Quaternion.RotateTowards(transform.rotation, transform.rotation * shoulderRotation, Time.deltaTime * speedRotation);

        // Jump:
        bool jump = skeletonMovement.GetBothArmUp();

        if (jump == true)    // se sto saltando
        {
            if (transform.position.y <= characterGroundHeight + 1f && jump != jumpPast)    // sono a terra e prima non stavo saltando
            {
                animator.SetBool("isJump", true);
                GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                jumpPast = true;
            }
            else
            {
                animator.SetBool("isJump", false);
            }
        }
        else
        {
            jumpPast = false;
        }

        transform.Translate(Vector3.right * Time.deltaTime * sideInclination * speedInclination); // applico spostamento laterale 
        transform.Translate(Vector3.forward * Time.deltaTime * speedMovement);   // sposto in avanti il personaggio
    }

    private void LateUpdate()       // aggiorno posizione rotazione camera per evitare scatti 
    {
        characterCamera.transform.position = this.transform.TransformPoint(cameraOffset);    // camera segue spostamento personaggio
        characterCamera.transform.LookAt(this.transform);                                  // camera segue rotazione personaggio
    }
}
