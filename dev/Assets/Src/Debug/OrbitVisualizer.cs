using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class OrbitVisualizer : MonoBehaviour
{
    public bool draw_orbit;
    public int time_into_future;
    private CelestialBody[] _bodies;
    private CelestialBody _this_body;
    private List<Vector3> _positions;

    // Start is called before the first frame update
    void Start(){
        GatherBodies();
        _this_body = gameObject.GetComponent(typeof(CelestialBody)) as CelestialBody;
        _positions = new List<Vector3>();
    }

    // Update is called once per frame
    void Update(){
    }

    void OnDrawGizmosSelected(){
        if (draw_orbit){
            // check if orbit needs to be recalculated and do so
            if (OrbitNeedsRecalculation()){
                CalculateOrbit();
            }

            // draw the orbit
            DrawOrbit();
        }
    }

    private void CalculateOrbit(){
        // clear lists and initialize position and velocity
        _positions.Clear();

        // temporary variables used to create positions
        Vector3 acceleration;
        Vector3 velocity;

        // init pos vel depending on if we're in the editor or not.
        _positions.Add(_this_body.position);
        if (Application.isPlaying){
            velocity = _this_body.velocity;
        }else{
            velocity = _this_body.initialVelocity;
        }

        // iterate forward using the simulation system and future timesteps
        for (int i = 0; i < time_into_future; i++){
            acceleration = SimulationSystem.CalculateAcceleration(_positions[i], _this_body); // get current acceleration
            velocity = velocity + (acceleration * Universe.physicsTimeStep); // update velocity 
            _positions.Add(_positions[i] + (velocity * Universe.physicsTimeStep)); // add new position
        }
    }

    private void DrawOrbit(){
        // now iterate down and draw it
        Vector3 prev = _positions[0];
        foreach (Vector3 pos in _positions){
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(prev, pos);
            prev = pos;
        }
    }

    private bool OrbitNeedsRecalculation(){
        if (_positions.Count <= 0){ // first check if there's no positions
            return true;
        }
        if (_positions[0] != _this_body.position){ // 
            return true;
        }
        if (_positions.Count != (time_into_future + 1)){
            return true;
        }
        return false;
    }

    // gathers celestial bodies from the scene.
    private void GatherBodies(){
        _bodies = FindObjectsOfType<CelestialBody>();
    }
    
}
