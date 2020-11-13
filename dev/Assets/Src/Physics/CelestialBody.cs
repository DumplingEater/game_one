using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof (Rigidbody))]
public class CelestialBody : MonoBehaviour{

    // relatively simple starting parameters
    public float radius;
    public float surfaceGravity;
    public Vector3 initialVelocity;
    public string bodyName = "Unnamed";
    Transform meshHolder;

    public Vector3 velocity {get; private set;}
    public float mass {get; private set;}
    Rigidbody rb;

    // called when the object is created
    void Awake(){
        rb = GetComponent<Rigidbody>();
        rb.mass = mass;
        velocity = initialVelocity;
    }

    // update velocity that is called from the sim controller
    public void UpdateVelocity (Vector3 acceleration, float timeStep) {
        velocity += acceleration * timeStep;
    }

    // called from sim controller to update position
    public void UpdatePosition(float timeStep){
        rb.MovePosition(rb.position + velocity * timeStep);
    }

    // called when a value is changed in the editor to validate properties
    void OnValidate(){
        mass = surfaceGravity * radius * radius / Universe.gravitationalConstant;
        meshHolder = transform.GetChild(0);
        meshHolder.localScale = Vector3.one * radius;
        gameObject.name = bodyName;
    }

    //
    // Properties
    //

    public Rigidbody Rigidbody {
        get {
            return rb;
        }
    }

    public Vector3 position {
        get {
            return rb.position;
        }
    }
}
