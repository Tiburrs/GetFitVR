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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private bool position3Next = false;

    // Reference to player camera to track phone angle
    public Camera playerCamera;
    // Flag to know whether the user is currently still working out. This is check in Update()
    private bool workoutInProgress = false;
    // Flag used to know whether to display final workout stats at the end of a workout in Update()
    private bool workoutDone = false;

    // ######## UI ########
    // Canvas items where the UI is placed that shows stats of the user's workout
    public Canvas workoutCanvas;
    public Text statsText;
    public Text timerText;
    public Text instructionText;
    private TimeSpan totalTime = new TimeSpan(0, 0, 0, 0);
    private TimeSpan bestTotalTime = new TimeSpan(0, 0, 0, 0);
    private TimeSpan bestSetTime = new TimeSpan(0, 0, 0, 0);
    private TimeSpan bestRepTime = new TimeSpan(0, 0, 0, 0);
    private bool firstWorkoutFrame = true;

    // ######## Extra Additions ##########
    public Canvas selections;
    void Start()
    {
        workoutCanvas.enabled = false;
        calibration = GetComponent<SensorCalibration>();

        // For testing purposes. Remove this for final prototype:
        
    }

    void Update()
    {
     
        //Debug.Log(GvrPointerInputModule.CurrentRaycastResult.gameObject.name);
    
        // Check if 1. a calibration has NOT been done AND 2. a calibration is NOT in progress
        if ((calibrated == false) && (calibration.userCalibrating == false) && workoutInProgress)
        {
            workoutCanvas.enabled = false;
            calibration.beginCalibration(selectedExercise);
        }
        else if(calibrated == true) // Else, check if a calibration has been done in the past
        {   // If so, continue to exercise
            if(workoutInProgress == true)
            {
                workoutCanvas.enabled = true;
                updateStatsText();
                // Check whether we're past the 1st frame here in order to enable the user to exit.
                // This is a hacky fix for a bug where the user got auto-exited on the first frame.
                if(firstWorkoutFrame == false)
                    if(Input.GetButtonDown("Fire1"))
                        finishWorkout();
                if(firstWorkoutFrame == true)
                    firstWorkoutFrame = false;
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
                                // Change trackers to track position 2 now
                                position1Next = false;
                                position2Next = true;
                            }
                            else if((position2Next == true) &&
                                ((playerCamera.transform.localEulerAngles.x > ((calibration.situpCalibratedRotations[2].x-errorPaddingDegrees)%360)) &&
                                (playerCamera.transform.localEulerAngles.x < ((calibration.situpCalibratedRotations[2].x+errorPaddingDegrees)%360))))  // Check for a position 2 angle
                            {
                                // 1 full rep has been down now, since this is position 2
                                // Change trackers to track position 1 now
                                position1Next = true;
                                position2Next = false;
                                repsSoFar += 1; // Increase rep count since we now did position 2, which is a full situp
                            }
                        }
                        else if(selectedExercise == ExerciseLibrary.Exercise.TwistCrunch)
                        {
                            // Wait on a new rep
                            if((position1Next == true) && 
                                ((playerCamera.transform.localEulerAngles.x > ((calibration.twistcrunchCalibratedRotations[1].x-errorPaddingDegrees)%360)) &&
                                (playerCamera.transform.localEulerAngles.x < ((calibration.twistcrunchCalibratedRotations[1].x+errorPaddingDegrees)%360))))   // Check for a position 1 angle
                            {
                                // Change trackers to track position 2 now
                                position1Next = false;
                                position2Next = true;
                                position1Next = false;
                            }
                            else if((position2Next == true) &&
                                        ((playerCamera.transform.localEulerAngles.x >
                                            ((calibration.twistcrunchCalibratedRotations[2].x-errorPaddingDegrees)%360)) &&
                                        (playerCamera.transform.localEulerAngles.x <
                                            ((calibration.twistcrunchCalibratedRotations[2].x+errorPaddingDegrees)%360)) &&
                                        (playerCamera.transform.localEulerAngles.y >
                                            ((calibration.twistcrunchCalibratedRotations[2].y-errorPaddingDegrees)%360)) &&
                                        (playerCamera.transform.localEulerAngles.y >
                                            ((calibration.twistcrunchCalibratedRotations[2].y-errorPaddingDegrees)%360))))  // Check for a position 2 angles
                            {
                                // 1 full rep has been down now, since this is position 2
                                // Change trackers to track position 3 now
                                position1Next = false;
                                position2Next = false;
                                position3Next = true;
                            }
                            else if((position3Next == true) &&
                                        ((playerCamera.transform.localEulerAngles.x >
                                            ((calibration.twistcrunchCalibratedRotations[3].x-errorPaddingDegrees)%360)) &&
                                        (playerCamera.transform.localEulerAngles.x <
                                            ((calibration.twistcrunchCalibratedRotations[3].x+errorPaddingDegrees)%360)) &&
                                        (playerCamera.transform.localEulerAngles.y >
                                            ((calibration.twistcrunchCalibratedRotations[3].y-errorPaddingDegrees)%360)) &&
                                        (playerCamera.transform.localEulerAngles.y >
                                            ((calibration.twistcrunchCalibratedRotations[3].y-errorPaddingDegrees)%360))))  // Check for a position 3 angles
                            {
                                // 1 full rep has been down now, since this is position 3
                                // Change trackers to track position 1 now
                                position1Next = true;
                                position2Next = false;
                                position3Next = false;
                                repsSoFar += 1; // Increase rep count since we now did position 2, which is a full twist crunch
                            }
                        }
                        else
                        {
                            Debug.Log("Workout Not Supported");
                        }
                    }
                    else
                    {
                        // Reset trackers in preparation for next set
                        repsSoFar = 0;
                        setsSoFar += 1;
                    }
                }
                else
                {
                    finishWorkout();
                }
            }
            else if(workoutDone == true)
            {
                finishWorkout();
                if(Input.GetButtonDown("Fire1"))
                    endWorkout();
            }
            else
                endWorkout();
        }
    }

    public void CallWorkout()
    {
        beginWorkout(ExerciseLibrary.Exercise.TwistCrunch);
    }
    public void beginWorkout(ExerciseLibrary.Exercise exerciseToCalibrate)
    {
        workoutCanvas.enabled = true;
        instructionText.text = "Press button on headset to STOP";
        workoutInProgress = true;
        workoutDone = false;
        firstWorkoutFrame = true;
        setsSoFar = 0;
        repsSoFar = 0;
        position1Next = true;
        selectedExercise = exerciseToCalibrate;
    }

    private void finishWorkout()
    {
        workoutInProgress = false;
        workoutDone = true;
        instructionText.text = "Done!\nPress button on headset to QUIT";
     
        // Show the final workout stats on the UI
        updateStatsText();
    }

    private void endWorkout()
    {
        workoutInProgress = false;
        workoutDone = false;
        workoutCanvas.enabled = false;
        calibrated = false;
        //enable to original trainer menus
        selections.enabled = true;
    }

    private void updateStatsText()
    {
        double calories = 0.25 * (repsSoFar + (setsSoFar * reps));
        statsText.text = "Sets: "+setsSoFar+"/"+sets+
                        "\nReps: "+repsSoFar+"/"+reps+
                        "\nCalories: "+calories;
    }
}
