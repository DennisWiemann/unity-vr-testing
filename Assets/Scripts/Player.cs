using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    public GameObject camPrefab;

    public float speed = 10.0f;
    private float translation;
    private float straffe;


    // Use this for initialization
    void Start () {
        if (isLocalPlayer) {
            GameObject cam = Instantiate(camPrefab, transform.position, Quaternion.identity);
            cam.transform.parent = transform;
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (!isLocalPlayer) {
            return;
        }

        // Input.GetAxis() is used to get the user's input
        // You can furthor set it on Unity. (Edit, Project Settings, Input)
        translation = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        straffe = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        transform.Translate(straffe, 0, translation);

        
    }
}
