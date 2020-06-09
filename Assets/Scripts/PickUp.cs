using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform pickUpHelper;
    public GameObject tempParent; 

    private bool isPicked = false;

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
