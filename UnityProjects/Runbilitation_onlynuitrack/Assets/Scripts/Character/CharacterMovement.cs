using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public Camera characterCamera;
    Vector3 cameraOffset;

    SkeletonMovement skeletonMovement;

    [SerializeField] public float speedMovement = 5f;
    [SerializeField] public float speedInclination = 2f;
    [SerializeField] public float speedRotation = 30f;

    private bool slideDownPast = false;

    // public float speed;

    private bool jump = false;
    private bool jumpPast = false;
    public float jumpForce;
    private bool isGrounded = false;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = characterCamera.transform.position - this.transform.position;  // vettore differenza tra camera e personaggio
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
        else if (slideDown == false)  // se non mi sto più inclinando aggiorno
        {
            slideDownPast = false;
        }

        // Rotation:
        Quaternion shoulderRotation = skeletonMovement.GetDeltaShoulderRotation();
        transform.rotation = Quaternion.RotateTowards(transform.rotation, transform.rotation * shoulderRotation, Time.deltaTime * speedRotation);

        // Jump:
        jump = skeletonMovement.GetBothArmUp();
        if (jump == true)
        {
            // Debug.Log("Salto");
            // Debug.Log("IsGrounded: "+ isGrounded);

            if (isGrounded && jump != jumpPast)
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

    void OnTriggerEnter(Collider c)
    {
        Ground ground = c.gameObject.GetComponent<Ground>();

        if (ground != null)     // when ground is hit
        {
            isGrounded = true;
        }
    }

    void OnTriggerExit(Collider c)
    {
        Ground ground = c.gameObject.GetComponent<Ground>();

        if (ground != null)      // when leave ground
        {
            isGrounded = false;
        }
    }

}
