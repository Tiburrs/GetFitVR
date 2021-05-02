﻿/* This is the script in charge of walking the user through their exercise sets
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
    private float startTime = -1;
    private TimeSpan bestTotalTime = new TimeSpan(0, 0, 0, 0);
    private TimeSpan bestSetTime = new TimeSpan(0, 0, 0, 0);
    private TimeSpan bestRepTime = new TimeSpan(0, 0, 0, 0);
    private bool firstWorkoutFrame = true;
    public GameObject situpStickFigure;
    public GameObject twistcrunchStickFigure;
    public Text repPercentage;
    public GameObject repPerBackground;

    // ######## Extra Additions ##########
    public Canvas selections;
    private Vector3 playerPos;
    private Vector3 calibratedPos1;
    private Vector3 calibratedPos2;
    private Vector3 calibratedPos3;
    void Start()
    {
        workoutCanvas.enabled = false;
        enableStickFigures(false);
        calibration = GetComponent<SensorCalibration>();
    }

    void Update()
    {
        // Check if 1. a calibration has NOT been done AND 2. a calibration is NOT in progress
        if ((calibrated == false) && (calibration.userCalibrating == false) && workoutInProgress)
        {
            workoutCanvas.enabled = false;
            enableStickFigures(false);
            calibration.beginCalibration(selectedExercise);
        }
        else if(calibrated == true) // Else, check if a calibration has been done in the past
        {   // If so, continue to exercise
            if(workoutInProgress == true)
            {
                workoutCanvas.enabled = true;
                enableStickFigures(true);
                updateTimerText();
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
                            // Normalize: To flip the 0-degree origin of the angles by 180 (makes comparing angles easier)
                            float playerX = normalizeLEAngle(playerCamera.transform.localEulerAngles.x);
                            float calibratedPos1X = normalizeLEAngle(calibration.situpCalibratedRotations[1].x);
                            float calibratedPos2X = normalizeLEAngle(calibration.situpCalibratedRotations[2].x);
                            moveStickFigureGuideSitup(playerX, calibratedPos1X, calibratedPos2X);
                            // Wait on a new rep
                            if((position1Next == true) && 
                                ((playerX > ((calibratedPos1X-errorPaddingDegrees))) &&
                                (playerX < ((calibratedPos1X+errorPaddingDegrees)))))   // Check for a position 1 angle
                            {
                                // Change trackers to track position 2 now
                                position1Next = false;
                                position2Next = true;
                            }
                            else if((position2Next == true) &&
                                ((playerX > ((calibratedPos2X-errorPaddingDegrees))) &&
                                (playerX < ((calibratedPos2X+errorPaddingDegrees)))))  // Check for a position 2 angle
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
                            // Normalize: To flip the 0-degree origin of the angles by 180 (makes comparing angles easier)
                            playerPos = new Vector3(normalizeLEAngle(playerCamera.transform.localEulerAngles.x),
                                                            normalizeLEAngle(playerCamera.transform.localEulerAngles.y), 0);
                            calibratedPos1 = new Vector3(normalizeLEAngle(calibration.twistcrunchCalibratedRotations[1].x),
                                                                normalizeLEAngle(calibration.twistcrunchCalibratedRotations[1].y), 0);
                            calibratedPos2 = new Vector3(normalizeLEAngle(calibration.twistcrunchCalibratedRotations[2].x),
                                                                normalizeLEAngle(calibration.twistcrunchCalibratedRotations[2].y), 0);
                            calibratedPos3 = new Vector3(normalizeLEAngle(calibration.twistcrunchCalibratedRotations[3].x),
                                                                normalizeLEAngle(calibration.twistcrunchCalibratedRotations[3].y), 0);
                            moveStickFigureGuideTwistcrunch(playerPos, calibratedPos1, calibratedPos2, calibratedPos3);
                            // Wait on a new rep
                            if((position1Next == true) && 
                                ((playerPos.x > (calibratedPos1.x-errorPaddingDegrees)) &&
                                (playerPos.x < (calibratedPos1.x+errorPaddingDegrees)) &&
                                (playerPos.y > (calibratedPos1.y-errorPaddingDegrees)) &&
                                (playerPos.y < (calibratedPos1.y+errorPaddingDegrees))))   // Check for a position 1 angle
                            {
                                // Change trackers to track position 2 now
                                position1Next = false;
                                position2Next = true;
                                position1Next = false;
                            }
                            else if((position2Next == true) &&
                                        ((playerPos.x > (calibratedPos2.x-errorPaddingDegrees)) &&
                                        (playerPos.x < (calibratedPos2.x+errorPaddingDegrees)) &&
                                        (playerPos.y > (calibratedPos2.y-errorPaddingDegrees)) &&
                                        (playerPos.y < (calibratedPos2.y+errorPaddingDegrees))))  // Check for a position 2 angles
                            {
                                // 1 full rep has been down now, since this is position 2
                                // Change trackers to track position 3 now
                                position1Next = false;
                                position2Next = false;
                                position3Next = true;
                            }
                            else if((position3Next == true) &&
                                        ((playerPos.x > (calibratedPos3.x-errorPaddingDegrees)) &&
                                        (playerPos.x < (calibratedPos3.x+errorPaddingDegrees)) &&
                                        (playerPos.y > (calibratedPos3.y-errorPaddingDegrees)) &&
                                        (playerPos.y < (calibratedPos3.y+errorPaddingDegrees))))  // Check for a position 3 angles
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
        beginWorkout(ExerciseLibrary.Exercise.TwistLunge);
    }
    public void beginWorkout(ExerciseLibrary.Exercise exerciseToCalibrate)
    {
        enableStickFigures(true);
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
        startTime = -1;
        enableStickFigures(false);
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

    /*  localEulerAngles range from 0 to 360. Once they go above 360, they restart at 0.
     *  This is a problem due to the nature of the character's camera in this app.
     *     When the character is looking straight forward at the horizon, its localEulerAngles.x is 0.
     *     If the character looks above, the angle immediately jumps to the 360s. This makes it difficult
     *     to compare angles when one is in the 360s and the other in the 10s.
     *  This function moves the 0 origin to the back of the character, an angle that the camera will
     *  never reach. Now, the localEulerAngle of 0 will become 180, making comparisons above and below
     *  the horizon easier.
     */
    private float normalizeLEAngle(float localEulerAngle)
    {
        return (localEulerAngle + 180) % 360;
    }

    private void moveStickFigureGuideSitup(float curPos, float lowerBound, float upperBound)
    {
        // Stick figure will move differently depending on the exercise
        if(selectedExercise == ExerciseLibrary.Exercise.SitUp)
        {
            Transform stickFigureTorso = situpStickFigure.transform.Find("UpperBody");
            SpriteRenderer pos1Line = situpStickFigure.transform.Find("Position1Line").GetComponent<SpriteRenderer>();
            SpriteRenderer pos2Line = situpStickFigure.transform.Find("Position2Line").GetComponent<SpriteRenderer>();
            // Change color of guide lines depending on next situp position
            if (position2Next == true)
            {
                pos1Line.color = new Color(0f, 1f, 0f, 1f);;
                pos2Line.color = new Color(1f, 0f, 0f, 1f);;
            }
            else if (position1Next == true)
            {
                pos1Line.color = new Color(1f, 0f, 0f, 1f);;
                pos2Line.color = new Color(0f, 1f, 0f, 1f);;
            }
            // Map the player's angle to the min and max angles of the stick figure (0 to -75 degrees)
            float stickFigureAngle = Mathf.Lerp(0, -75, Mathf.InverseLerp(lowerBound, upperBound, curPos));
            // Move the stick figure
            stickFigureTorso.localEulerAngles = new Vector3(
                stickFigureTorso.localEulerAngles.x,
                stickFigureTorso.localEulerAngles.y,
                stickFigureAngle);
            // Calculate the percentage of the rep we're at
            int repPercent = 0;
            stickFigureAngle = Mathf.Lerp(0, -75, Mathf.InverseLerp(lowerBound+errorPaddingDegrees, upperBound-errorPaddingDegrees, curPos));
            // Add 50% if we're past position 1
            if (position2Next == true)
                repPercent = 50 + (int) Math.Round(Mathf.Lerp(0, 50, Mathf.InverseLerp(0, -75, stickFigureAngle)), 0);
            else
                repPercent = (int) Math.Round(Mathf.Lerp(50, 0, Mathf.InverseLerp(0, -75, stickFigureAngle)), 0);
            repPercentage.text = "Rep: "+repPercent+"%";
        }
    }

    private void moveStickFigureGuideTwistcrunch(Vector3 playerPos, Vector3 calibratedPos1, Vector3 calibratedPos2, Vector3 calibratedPos3)
    {
        if(selectedExercise == ExerciseLibrary.Exercise.TwistCrunch)
        {
            Transform stickFigureTorso = twistcrunchStickFigure.transform.Find("UpperBody");
            SpriteRenderer pos1Line = twistcrunchStickFigure.transform.Find("Position1Line").GetComponent<SpriteRenderer>();
            SpriteRenderer pos2Line = twistcrunchStickFigure.transform.Find("Position2Line").GetComponent<SpriteRenderer>();
            SpriteRenderer pos3Line = twistcrunchStickFigure.transform.Find("Position3Line").GetComponent<SpriteRenderer>();
            // Change color of guide lines depending on next situp position. Also map the player's min/max angles to the stick figure
            if (position2Next == true)
            {
                pos1Line.color = new Color(0f, 1f, 0f, 1f);
                pos2Line.color = new Color(1f, 0f, 0f, 1f);
                pos3Line.color = new Color(1f, 0f, 0f, 1f);
                
            }
            else if (position3Next == true)
            {
                pos1Line.color = new Color(0f, 1f, 0f, 1f);
                pos2Line.color = new Color(0f, 1f, 0f, 1f);
                pos3Line.color = new Color(1f, 0f, 0f, 1f);
            }
            else if (position1Next == true)
            {
                pos1Line.color = new Color(1f, 0f, 0f, 1f);
                pos2Line.color = new Color(0f, 1f, 0f, 1f);
                pos3Line.color = new Color(0f, 1f, 0f, 1f);
            }
            // Map the player's angle to the min and max angles of the stick figure
            float stickFigureZ = Mathf.Lerp(0, -30, Mathf.InverseLerp(calibratedPos1.x+errorPaddingDegrees, Math.Max(calibratedPos2.x, calibratedPos3.x)-errorPaddingDegrees, playerPos.x));
            float stickFigureY = Mathf.Lerp(-35, 35, Mathf.InverseLerp(calibratedPos2.y+errorPaddingDegrees, calibratedPos3.y-errorPaddingDegrees, playerPos.y));
            // Move the stick figure
            stickFigureTorso.localEulerAngles = new Vector3(
                stickFigureTorso.localEulerAngles.x,
                stickFigureY,
                stickFigureZ);
            // Calculate the percentage of the rep we're at
            int repPercent = 0;
            // Add 33% if we're past position 1
            if (position2Next == true)
            {
                repPercent = 33 + (int) Math.Round(Mathf.Lerp(0, 33/2f, Mathf.InverseLerp(calibratedPos1.x+errorPaddingDegrees, calibratedPos2.x-errorPaddingDegrees, playerPos.x)) +
                                        Mathf.Lerp(0, 33/2f, Mathf.InverseLerp(calibratedPos1.y+errorPaddingDegrees, calibratedPos2.y-errorPaddingDegrees, playerPos.y)), 0);
            }
            else if (position3Next == true)
            {
                repPercent = 66 + (int) Math.Round(Mathf.Lerp(0, 33/2f, Mathf.InverseLerp(calibratedPos1.x+errorPaddingDegrees, calibratedPos3.x-errorPaddingDegrees, playerPos.x)), 0) +
                                    (int) Math.Round(Mathf.Lerp(0, 33/2f, Mathf.InverseLerp(calibratedPos2.y+errorPaddingDegrees, calibratedPos3.y-errorPaddingDegrees, playerPos.y)), 0);
            }
            else
            {
                repPercent = (int) Math.Round(Mathf.Lerp(33, 0, Mathf.InverseLerp(calibratedPos1.x+errorPaddingDegrees, Math.Max(calibratedPos2.x, calibratedPos3.x)-errorPaddingDegrees, playerPos.x)), 0);
            }
            repPercentage.text = "Rep: "+repPercent+"%";
        }
    }

    private void updateTimerText()
    {
        if (startTime == -1)
            startTime = Time.time;
        TimeSpan elapsedTime = TimeSpan.FromSeconds(Time.time - startTime);
        string timeText = string.Format("{0:D2}:{1:D2}.{2:D2}", elapsedTime.Minutes, elapsedTime.Seconds, elapsedTime.Milliseconds/10);
        timerText.text = "Time:\n"+timeText;
    }

    private void enableStickFigures(bool enabled)
    {
        if (enabled == false)
        {
            repPerBackground.SetActive(enabled);
            situpStickFigure.SetActive(enabled);
            twistcrunchStickFigure.SetActive(enabled);
        }
        else
        {
            repPerBackground.SetActive(enabled);
            if (selectedExercise == ExerciseLibrary.Exercise.SitUp)
            {
                situpStickFigure.SetActive(enabled);
                twistcrunchStickFigure.SetActive(false);
            }
            else if (selectedExercise == ExerciseLibrary.Exercise.TwistCrunch)
            {
                situpStickFigure.SetActive(false);
                twistcrunchStickFigure.SetActive(enabled);
            }
        }
    }
}
