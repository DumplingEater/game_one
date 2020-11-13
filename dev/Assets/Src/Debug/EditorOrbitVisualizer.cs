using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EditorOrbitVisualizer : MonoBehaviour
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
        if (draw_orbit){
            _positions.Clear();
            _velocities.Clear();
            _accelerations.Clear();
            _positions.Add(_this_body.position);
            _velocities.Add(_this_body.velocity);

            for (int i = 0; i < time_into_future; i++){
                _accelerations.Add(SimulationSystem.CalculateAcceleration(_positions[i], _this_body));
                _velocities.Add(_velocities[i] + _accelerations[i]);
                _positions.Add(_positions[i] + _velocities[i]);
                Debug.Log(_positions[i]);
            }
        }
    }

    private void DrawOrbit(){
        
    }

    // gathers celestial bodies from the scene.
    private void GatherBodies(){
        _bodies = FindObjectsOfType<CelestialBody>();
    }

    
}
