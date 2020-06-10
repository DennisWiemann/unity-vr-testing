﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Cam : MonoBehaviour
{

	[SerializeField]
    public float sensitivity = 5.0f;
    [SerializeField]
    public float smoothing = 2.0f;
    // the chacter is the capsule
    private GameObject character;
    // get the incremental value of mouse moving
    private Vector2 mouseLook;
    // smooth the mouse moving
    private Vector2 smoothV;


    // Start is called before the first frame update
    void Start()
    {
        character = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // if (!character.GetComponent<NetworkIdentity>().isLocalPlayer) {
        //     enabled = false;
        //     return;
        // }

        if (Input.GetKey(KeyCode.R)) {
            return;
        }

        // md is mosue delta
        var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        // the interpolated float result between the two float values
        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
        // incrementally add to the camera look
        mouseLook += smoothV;

        // vector3.right means the x-axis
        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);
    }
}
