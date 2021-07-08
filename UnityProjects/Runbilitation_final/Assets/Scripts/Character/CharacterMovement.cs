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
        skeletonMovement = GameObject.FindGameObjectWithTag("UserMovements").GetComponent<SkeletonMovement>();
        animator = gameObject.GetComponent<Animator>();
        // check if the reference is valid
        Debug.Assert(skeletonMovement != null);
    }


    // Update is called once per frame
    void Update()
    {
        // left or right movement
        float sideInclination = skeletonMovement.GetSideInclination();

        // slide the character down
        bool slideDown = skeletonMovement.GetBackInclination();
        if (slideDown == true)   // se mi devo inclinare e prima non lo stavo facendo
        {
            if (slideDown != slideDownPast)
            {
                animator.SetBool("isSlide", true);       // animazione inclinazione
                transform.Translate(0, 0, 0.1f);         // inclinazione
                slideDownPast = true;
            }
            else
            {
                animator.SetBool("isSlide", false);
            }
        }
        else if (slideDown == false)  // se non mi sto pi? inclinando aggiorno
        {
            slideDownPast = false;
        }

        // Rotation:
        Quaternion shoulderRotation = skeletonMovement.GetDeltaShoulderRotation();
        transform.rotation = Quaternion.RotateTowards(transform.rotation, transform.rotation * shoulderRotation, Time.deltaTime * speedRotation);

        // Jump:
        bool jump = skeletonMovement.GetBothArmUp();
        if (jump == true)
        {
            // Debug.Log("Salto");
            // Debug.Log("IsGrounded: "+ isGrounded);

            if (transform.position.y <= characterGroundHeight + 1f && jump != jumpPast)
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

        transform.Translate(Vector3.right * Time.deltaTime * sideInclination * speedInclination); // deltaTime per spostamenti fluidi; vector3 right: direzione x 
        transform.Translate(Vector3.forward * Time.deltaTime * speedMovement);
    }

    private void LateUpdate()
    {
        characterCamera.transform.position = this.transform.TransformPoint(cameraOffset);
        characterCamera.transform.LookAt(this.transform);
    }
}
