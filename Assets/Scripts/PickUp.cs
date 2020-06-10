using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PickUp : MonoBehaviour
{
    public float rotationSpeed = 20f;
    public float tolerance = 15;

    private Transform pickUpHelper;
    private GameObject tempParent; 
    

    private bool isPicked = false;
    private bool isSnapped = false;

    private Collider other;


   void Start()
   {
        
   }

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
        Debug.Log("Clicked!");

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
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
            if (player.GetComponent<NetworkIdentity>().isLocalPlayer) {
                Debug.Log("Player detected");

                pickUpHelper = FindGameObjectInChildWithTag(player, "MainCamera").transform.Find("PickUpHelper");

                isPicked = true;
                transform.GetComponent<Rigidbody>().useGravity = false;
                transform.GetComponent<Rigidbody>().isKinematic = true;

                transform.position = pickUpHelper.transform.position;
                transform.rotation = pickUpHelper.transform.rotation;

                transform.parent = pickUpHelper.transform;
            }
        
        }
        
    }

    void Drop()
    {
        isPicked = false;
        transform.GetComponent<Rigidbody>().useGravity = true;
        transform.GetComponent<Rigidbody>().isKinematic = false;

        transform.parent = null;
        transform.position = pickUpHelper.transform.position;

        pickUpHelper = null;
    }

    bool canSnap()
    {
        //get eulers from this object and from the collider
        Vector3 otherEuler = other.transform.rotation.eulerAngles;
        Vector3 meEuler = transform.rotation.eulerAngles;

        Debug.Log("other x: " + otherEuler.x + " me: " + meEuler.x);
        Debug.Log("other y: " + otherEuler.y + " me: " + meEuler.y);
        Debug.Log("other z: " + otherEuler.z + " me: " + meEuler.z);

        float xDiff = Mathf.DeltaAngle(otherEuler.x, meEuler.x);
        float yDiff = Mathf.DeltaAngle(otherEuler.x, meEuler.x);
        float zDiff = Mathf.DeltaAngle(otherEuler.x, meEuler.x);

        Debug.Log("xDiff " + xDiff);
        Debug.Log("yDiff " + yDiff);
        Debug.Log("zDiff " + zDiff);

        //check are axes correct
        if (Mathf.Abs(xDiff) < tolerance && Mathf.Abs(yDiff) < tolerance && Mathf.Abs(zDiff) < tolerance) {
            
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

    public static GameObject FindGameObjectInChildWithTag (GameObject parent, string tag)
    {
        Transform t = parent.transform;

        for (int i = 0; i < t.childCount; i++) 
        {
            if(t.GetChild(i).gameObject.tag == tag)
            {
                return t.GetChild(i).gameObject;
            }
                
        }
            
        return null;
    }
    

}
