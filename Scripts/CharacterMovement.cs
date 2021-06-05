using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public Camera camera;
    Vector3 cameraOffset;

    SkeletonMovement skeletonMovement = new SkeletonMovement();

    [SerializeField] public float threshold = 0.5f;
    [SerializeField] public float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = camera.transform.position - this.transform.position;  // vettore differenza tra camera e personaggio
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentUserTracker.CurrentUser != 0)     // se rileva lo skeleton
        {
            nuitrack.Skeleton skeleton = CurrentUserTracker.CurrentSkeleton;
            float verticalAxis = skeletonMovement.GetVerticalAxis(skeleton);
            if (verticalAxis > threshold || verticalAxis < -threshold)
                this.transform.Translate(Vector3.right * Time.deltaTime * verticalAxis); // verticalAxis indica la quantità di spostamento, deltaTime per spostamenti fluidi; vector3 right: direzione x 

            this.transform.Translate(Vector3.forward * Time.deltaTime * speed);
                        
        }
    }


    private void LateUpdate()
    {
        camera.transform.position = this.transform.position + cameraOffset;
    }
}
