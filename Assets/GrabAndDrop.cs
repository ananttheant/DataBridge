﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class GrabAndDrop : MonoBehaviour {

    private GameObject grabbedObject;
    private float grabbedObjectSize;
    private RigidbodyFirstPersonController controller;
    private static readonly Vector3 adjustTheHeightOfTheObject = new Vector3(-0.5f, -0.5f, 0);

    void Start()
    {
        controller = GetComponent<RigidbodyFirstPersonController>();
    }

    //function that gives us the object that we are looking at
    GameObject GetMouseHoverObject (float range)
    {
        Vector3 position = gameObject.transform.position;
        RaycastHit raycastHit;
        Vector3 target = position + Camera.main.transform.forward * range;
        if (Physics.Linecast(position, target, out raycastHit))
        {
            return raycastHit.collider.gameObject;
        }
        // if theres no collision then the code will get down further
        return null;

    }

    //void changeIsGrabbing()
    //{


    //    gameManager.isGrabbing = true;
    //}




    void tryGrabObject(GameObject grabObject)
    {
        // check if actually a thing that we can grab
        if (grabObject == null || !CanGrab(grabObject)) // if it's nothing then u cant grab it and return
            return;

        grabbedObject = grabObject;
        grabbedObjectSize = grabObject.GetComponent<Renderer>().bounds.size.magnitude;
        grabObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        controller.movementSettings.ForwardSpeed = 0;
        controller.movementSettings.BackwardSpeed = 0;
        controller.movementSettings.StrafeSpeed = 0;
        controller.movementSettings.JumpForce = 0;



        GameStates.isGrabbing = true;
        
    }



    bool CanGrab(GameObject candidate) // we an grab the object if it has a rigid body
    {
        return candidate.GetComponent<Rigidbody>() != null;// if we find the rigid body, return true
    }



    void DropObject()
    {
       
        if (grabbedObject == null) // if nothing is grabbed then 
            return;




        if (grabbedObject.GetComponent<Rigidbody>() != null) // if the grabbed object has a rigid body, not null 
        {
            grabbedObject.GetComponent<Rigidbody>().velocity = Vector3.zero; // TODO
            grabbedObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            controller.movementSettings.ForwardSpeed = Constants.DefaultForwardSpeed;
            controller.movementSettings.BackwardSpeed = Constants.DefaultBackwardSpeed;
            controller.movementSettings.StrafeSpeed = Constants.DefaultStrafeSpeed;
            controller.movementSettings.JumpForce = Constants.DefaultJumpForce;

            GameStates.isGrabbing = false; 


        }
        grabbedObject = null;


    }


	// Update is called once per frame
	void Update () {
        //Debug.Log(GetMouseHoverObject(5));

        if (Input.GetMouseButtonDown(0))
        {
            if (grabbedObject == null) //if havent grabbed any object, grab one
            {
                tryGrabObject(GetMouseHoverObject(4));
            }
            else
            {
                DropObject(); //if we alredy have something on our hand then drop it
            }
        }

        if (grabbedObject != null)//if we have grabbed an object , change its position to in front of us
        {
            Vector3 newPosition = gameObject.transform.position + Camera.main.transform.forward * (grabbedObjectSize) + (Vector3.up - adjustTheHeightOfTheObject );
            grabbedObject.transform.position = newPosition  ;
        }
	}
}
