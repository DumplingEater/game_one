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
    private List<Vector3> _velocities;
    private List<Vector3> _accelerations;
    // Start is called before the first frame update
    void Start(){
        GatherBodies();
        _this_body = gameObject.GetComponent(typeof(CelestialBody)) as CelestialBody;
        _positions = new List<Vector3>();
        _velocities = new List<Vector3>();
        _accelerations = new List<Vector3>();
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
        _velocities.Clear();
        _accelerations.Clear();
        _positions.Add(_this_body.position);
        _velocities.Add(_this_body.velocity);

        // iterate forward using the simulation system and future timesteps
        for (int i = 0; i < time_into_future; i++){
            _accelerations.Add(SimulationSystem.CalculateAcceleration(_positions[i], _this_body));
            _velocities.Add(_velocities[i] + (_accelerations[i] * Universe.physicsTimeStep));
            _positions.Add(_positions[i] + (_velocities[i] * Universe.physicsTimeStep));
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
        if (_velocities[0] != _this_body.velocity){
            return true;
        }
        return false;
    }

    // gathers celestial bodies from the scene.
    private void GatherBodies(){
        _bodies = FindObjectsOfType<CelestialBody>();
    }
    
}
