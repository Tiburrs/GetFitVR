using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkoutOptions : MonoBehaviour
{
    // Reference the to canvas containing the workouts
    public Canvas workoutOptionsCanvas;

    void Start()
    {
        workoutOptionsCanvas.enabled = false;
    }

    void Update()
    {
        if(workoutOptionsCanvas.enabled == true)
        {
            // Listen for the user tapping the screen to exit the workoutOptionsCanvas
            if(Input.GetButtonDown("Fire1"))
                workoutOptionsCanvas.enabled = false;
        }
    }

    // Function called by the onClick event trigger of the "Workout" button being gazed at.
    public void CallWorkoutOptions()
    {
        workoutOptionsCanvas.enabled = true;
    }
}
