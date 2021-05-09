using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkoutOptions : MonoBehaviour
{
    // Reference the to canvas containing the workouts
    public Canvas workoutOptionsCanvas;
    bool on;
    private Canvas trainerOptionsCanvas;
    void Start()
    {
        on = false;
        workoutOptionsCanvas.enabled = on;
        trainerOptionsCanvas = GameObject.Find("Trainer Options").GetComponent<Canvas>();
    }

    void Update()
    {
        if(workoutOptionsCanvas.enabled == true)
        {
            // Listen for the user tapping the screen to exit the workoutOptionsCanvas
            if(Input.GetButtonDown("Fire1"))
            {
                on = !on;
                workoutOptionsCanvas.enabled = false;
                trainerOptionsCanvas.enabled = true;
            }
        }
    }

    // Function called by the onClick event trigger of the "Workout" button being gazed at.
    public void CallWorkoutOptions()
    {
        on = !on;
        workoutOptionsCanvas.enabled = on;
        //Debug.Log(on);
    }
}
