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
    private Vector3 _initial_velocity;

    // Start is called before the first frame update
    void Start(){
        GatherBodies();
        _this_body = gameObject.GetComponent(typeof(CelestialBody)) as CelestialBody;
        _positions = new List<Vector3>();

        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>() as LineRenderer;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 5.0f;
        lineRenderer.positionCount = time_into_future;
    }

    // Update is called once per frame
    void Update(){
        if (draw_orbit)
        {
            // check if orbit needs to be recalculated and do so
            if (OrbitNeedsRecalculation())
            {
                CalculateOrbit();
            }

            // draw the orbit
            DrawOrbit();
        }
    }

    private void CalculateOrbit(){
        // clear position and line renderer
        _positions.Clear();
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;

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

        _initial_velocity = velocity;

        // iterate forward using the simulation system and future timesteps
        for (int i = 0; i < time_into_future; i++){
            acceleration = SimulationSystem.CalculateAcceleration(_positions[i], _this_body); // get current acceleration
            velocity = velocity + (acceleration * Universe.physicsTimeStep); // update velocity 
            _positions.Add(_positions[i] + (velocity * Universe.physicsTimeStep)); // add new position
        }
    }

    private void DrawOrbit(){
        // update line renderer positions count
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = time_into_future;

        // now iterate down and draw it
        lineRenderer.SetPositions(_positions.ToArray());
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
        if (Application.isPlaying){
            if (_initial_velocity != _this_body.velocity){
                return true;
            }
        }else{
            if (_initial_velocity != _this_body.initialVelocity){
                return true;
            }
        }
        return false;
    }

    // gathers celestial bodies from the scene.
    private void GatherBodies(){
        _bodies = FindObjectsOfType<CelestialBody>();
    }
    
}
