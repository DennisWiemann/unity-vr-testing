using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform pickUpHelper;
    public GameObject tempParent; 

    public float rotationSpeed = 5f;

    private bool isPicked = false;

    void Update()
    {
        if (isPicked) {
            if (Input.GetKey(KeyCode.R))
            {
                float scroll = Input.GetAxis("Mouse ScrollWheel");
                float mouseDX = Input.GetAxis("Mouse X");
                float mouseDY = Input.GetAxis("Mouse Y");

                Vector3 axis = new Vector3(mouseDX, scroll, mouseDY);

                Quaternion from = transform.rotation;
                Quaternion to = transform.rotation;
                to *= Quaternion.Euler( axis * 45 );
                transform.rotation = to;
            }
            
        }
    }

   

    void OnMouseDown() 
    {
        if (isPicked) {
            isPicked = false;

            transform.GetComponent<Rigidbody>().useGravity = true;
            transform.GetComponent<Rigidbody>().isKinematic = false;

            transform.parent = null;
            transform.position = pickUpHelper.transform.position;
        } else {
            isPicked = true;

            transform.GetComponent<Rigidbody>().useGravity = false;
            transform.GetComponent<Rigidbody>().isKinematic = true;
            transform.position = pickUpHelper.transform.position;
            transform.rotation = pickUpHelper.transform.rotation;

            transform.parent = tempParent.transform;
        }
        
    }

}
