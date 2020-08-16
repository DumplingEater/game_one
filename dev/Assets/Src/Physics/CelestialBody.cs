using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBody : MonoBehaviour{

    // relatively simple starting parameters
    public float mass;
    public float radius;
    public Vector3 initialVelocity;
    Vector3 currentVelocity;

    // Start is called before the first frame update
    void Start(){

    }

    // called when the object is created
    void Initialize(){
        currentVelocity = initialVelocity;
    }

    // called each frame to update the velocity
    void UpdateVelocity(CelestialBody[] bodies, float dt){
        foreach(var body in bodies){ // for each body passed in
            if(body != this){ // if it's not this body
                float sqrDst = (body.GetComponent<Rigidbody>().position - GetComponent<Rigidbody>().position).sqrMagnitude;
                Vector3 forceDir = (body.GetComponent<Rigidbody>().position - GetComponent<Rigidbody>().position).normalized;
                Vector3 acceleration = forceDir * Universe.gravitationalConstant * body.mass / sqrDst;
                currentVelocity += acceleration * dt;
            }
        }
    }

    // Update is called once per frame
    void Update(){

    }
}
