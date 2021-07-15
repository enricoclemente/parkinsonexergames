using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Camera characterCamera;
    Vector3 cameraOffset;
    float characterGroundHeight;

    [SerializeField] public float jumpForce = 0.5f;
    [SerializeField] public float speedMovement = 5f;


    // Start is called before the first frame update
    void Start()
    {
        characterGroundHeight = transform.position.y; 
        cameraOffset = characterCamera.transform.position - this.transform.position;  // vettore differenza tra camera e personaggio
    }


    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.Rotate(0f, -90.0f, 0f, Space.Self);
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            transform.Rotate(0f, 90.0f, 0f, Space.Self);
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (transform.position.y <= characterGroundHeight+1f)
                GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            else
                Debug.Log("non posso saltare!");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            speedMovement = 0;
        }

        this.transform.Translate(Vector3.forward * Time.deltaTime * speedMovement);
        this.transform.Translate(Vector3.left * Time.deltaTime * speedMovement * -horizontalInput);
    }


    void LateUpdate()
    {
        characterCamera.transform.position = this.transform.TransformPoint(cameraOffset);
        characterCamera.transform.LookAt(this.transform);
    }
}
