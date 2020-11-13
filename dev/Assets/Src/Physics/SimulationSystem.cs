using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SimulationSystem : MonoBehaviour
{
    CelestialBody[] bodies;
    static SimulationSystem instance;

    // called on script creation
    void Awake(){
          bodies = FindObjectsOfType<CelestialBody> ();
          Time.fixedDeltaTime = Universe.physicsTimeStep;
          Debug.Log("Setting fixedDeltaTime to: "+Time.fixedDeltaTime);
    }

    // Fixed Update is called every time.dixedDeltaTime and is independent of framerate
    void FixedUpdate(){
        // calculate and apply acceleration to each body
        for (int i = 0; i < bodies.Length; i++){
            Vector3 acceleration = CalculateAcceleration(bodies[i].position, bodies[i]);
            bodies[i].UpdateVelocity(acceleration, Universe.physicsTimeStep); //  do we need to access this off of universe?
        }

        // use updated velocity to update positions
        for (int i = 0; i < bodies.Length; i++){
            bodies[i].UpdatePosition(Universe.physicsTimeStep);
        }
    }

    // calculate acceleration for a body
    public static Vector3 CalculateAcceleration(Vector3 point, CelestialBody ignoreBody = null){
        Vector3 acceleration = Vector3.zero; // init acceleration to 0
        foreach(var body in Instance.bodies){ // iterate over all the bodies
            if(body != ignoreBody){ // if we're not ignoring this body
                float sqrDst = (body.position - point).sqrMagnitude; // square distance
                Vector3 forceDir = (body.position - point).normalized; // displacement vector
                acceleration += forceDir * Universe.gravitationalConstant * body.mass / sqrDst; // newton's g law
            }
        }
        return acceleration;
    }

    //
    // Properties
    //

    // body list property
    public static CelestialBody[] Bodies {
        get {
            return Instance.bodies;
        }
    }

    // instance property
    static SimulationSystem Instance{
        get{
            if (instance == null){
                instance = FindObjectOfType<SimulationSystem> ();
            }
            return instance;
        }
    }
}
