using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform pickUpHelper;
    public GameObject tempParent; 
    public float rotationSpeed = 20f;
    private bool isPicked = false;
    private Collider other;
    public float tolerance = 15;
    private bool isSnapped = false;

    void Update()
    {
        if (isPicked) {
            //if you hold R, you can rotate.
            if (Input.GetKey(KeyCode.R))
            {
                Rotate();
            }
        }
    }

    void OnMouseDown() 
    {
        if (!isSnapped) 
        {
            if (other != null) 
            {
                Debug.Log("other != null");
                //if you can snap, do nothing else
                if (canSnap()) {
                    return;
                }
            }
        }
        
        isSnapped = false;
        
        if (isPicked) 
        {
            Drop();
        }  else {
            Pick();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        
        Debug.Log(other.tag);
        if (other.tag == "SnapPlace") 
        {
            this.other = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit");
        this.other = null;
    }

    void Rotate()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float mouseDX = Input.GetAxis("Mouse X");
        float mouseDY = Input.GetAxis("Mouse Y");

        Vector3 axis = new Vector3(mouseDX, scroll, mouseDY);

        Quaternion from = transform.rotation;
        Quaternion to = transform.rotation;
        to *= Quaternion.Euler(axis * rotationSpeed);
        transform.rotation = to;
    }

    void Pick()
    {
        isPicked = true;
        transform.GetComponent<Rigidbody>().useGravity = false;
        transform.GetComponent<Rigidbody>().isKinematic = true;
        transform.position = pickUpHelper.transform.position;
        transform.rotation = pickUpHelper.transform.rotation;

        transform.parent = tempParent.transform;
    }

    void Drop()
    {
        isPicked = false;
        transform.GetComponent<Rigidbody>().useGravity = true;
        transform.GetComponent<Rigidbody>().isKinematic = false;

        transform.parent = null;
        transform.position = pickUpHelper.transform.position;
    }

    bool canSnap()
    {
        //get eulers from this object and from the collider
        Vector3 otherEuler = other.transform.rotation.eulerAngles;
        Vector3 meEuler = transform.rotation.eulerAngles;

        //check are axes correct
        if (Mathf.Abs(otherEuler.x - meEuler.x) < tolerance && Mathf.Abs(otherEuler.y - meEuler.y) < tolerance && Mathf.Abs(otherEuler.z - meEuler.z) < tolerance) {
            
            Debug.Log("Can Snap!");

            isPicked = false;
            isSnapped = true;

            transform.GetComponent<Rigidbody>().useGravity = false;
            transform.GetComponent<Rigidbody>().isKinematic = true;

            transform.position = other.transform.position;
            transform.rotation = other.transform.rotation;
            transform.parent = null;

            other = null;

            return true;
        }

        return false;
    }

    

}
