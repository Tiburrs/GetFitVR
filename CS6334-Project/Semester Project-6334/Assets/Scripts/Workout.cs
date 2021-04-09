/* This is the script in charge of walking the user through their exercise sets
 * and reps for 1 selected workout (ie situp). It first checks whether a
 * calibration has been done. If not, it launches a calibration from
 * SensorCalibration.cs and waits for it to finish.
 * 
 * It also has a default margin-of-error (degrees) that can be changed by the developer.
 * This margin is added and subtracted (during exercies) to the angles set from the user
 * during calibration.
 *
 * The number of sets and reps can also be adjusted manually or via a menu.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workout : MonoBehaviour
{
    /* ######## CALIBRATING ######## */
    // Flag to know if a calibration has been done for this exercise. This flag is set by SensorCalibration.cs on completion.
    public bool calibrated = false;
    // Reference to calibration script to check if a calibration is in progress
    private SensorCalibration calibration;

    /* ######## WORKOUT SELECTION ######## */
    // The exercise the user selected in the menu
    public ExerciseLibrary.Exercise selectedExercise = ExerciseLibrary.Exercise.SitUp;
    // Number of sets this workout of "selectedExercise" will last
    public int sets = 2;
    // Number of reps per set in this workout
    public int reps = 5;

    /* ######## TRACKING ######## */
    // Margin-of-error to be added and subtracted to angles found during calibration
    public float errorPaddingDegrees = 5;
    // Trackers to know where we are in the workout
    private int setsSoFar = 0;
    private int repsSoFar = 0;
    // Flags to know which position of the situp is next to track
    private bool position1Next = true;
    private bool position2Next = false;
    // Reference to player camera to track phone angle
    public Camera playerCamera;

    void Start()
    {
        calibration = GetComponent<SensorCalibration>();
    }

    void Update()
    {
        // Check if 1. a calibration has NOT been done AND 2. a calibration is NOT in progress
        if((calibrated == false) && (calibration.userCalibrating == false))
            calibration.beginCalibration(selectedExercise);
        else if(calibrated == true) // Else, check if a calibration has been done in the past
        {   // If so, continue to exercise

            // Check if entire exercise is done yet
            if(setsSoFar < sets)
            {
                if(repsSoFar < reps)
                {
                    // Here, there is different logic for each type of exercise, since all have different numbers of positions
                    if(selectedExercise == ExerciseLibrary.Exercise.SitUp)
                    {
                        // Wait on a new rep
                        if((position1Next == true) && 
                            ((playerCamera.transform.localEulerAngles.x > ((calibration.situpCalibratedRotations[1].x-errorPaddingDegrees)%360)) &&
                            (playerCamera.transform.localEulerAngles.x < ((calibration.situpCalibratedRotations[1].x+errorPaddingDegrees)%360))))   // Check for a position 1 angle
                        {
                            Debug.Log("Position 1 done");
                            // Change trackers to track position 2 now
                            position1Next = false;
                            position2Next = true;
                        }
                        else if((position2Next == true) &&
                            ((playerCamera.transform.localEulerAngles.x > ((calibration.situpCalibratedRotations[2].x-errorPaddingDegrees)%360)) &&
                            (playerCamera.transform.localEulerAngles.x < ((calibration.situpCalibratedRotations[2].x+errorPaddingDegrees)%360))))  // Check for a position 2 angle
                        {
                            Debug.Log("Position 2 done");
                            // Change trackers to track position 1 now
                            position1Next = true;
                            position2Next = false;
                            repsSoFar += 1; // Increase rep count since we now did position 2, which is a full situp
                        }
                    }
                    else
                    {
                        Debug.Log("Workout Not Supported");
                    }
                }
                else
                {
                    Debug.Log("Set done");
                    // Reset trackers in preparation for next set
                    repsSoFar = 0;
                    setsSoFar += 1;
                }
            }
            else
            {
                // Workout complete
                Debug.Log("Workout Complete");
            }
        }
    }

    /*  For easy range comparison between angles, change eular angles of values [360,180) to [0,-180).
     *  Eular values in the range [0,180] will remain the same.
     *  This conversion makes 
     */
    private float numberLineAngle(float eulerAngle)
    {
        if(eulerAngle > 180)
            return -(360 - eulerAngle);
        else
            return eulerAngle;
    }
}
