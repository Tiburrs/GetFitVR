/* This is the script in charge of walking the user through calibrating their phone's
 * rotation in preparation for the various supported exercises.
 *
 * To start a calibration walkthough, call beginCalibration() from an outside script.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensorCalibration : MonoBehaviour
{
    // Canvas items where the UI is placed that walks the user through the calibration process'
    public Canvas calibrationCanvas;
    public RawImage instructionalImage;
    public Text instructionalText;
    public Texture situpPosition1Image;
    public Texture situpPosition2Image;
    public Texture twistcrunchPosition1Image;
    public Texture twistcrunchPosition2Image;
    public Texture twistcrunchPosition3Image;
    public Texture twistlungePosition1Image;
    public Texture twistlungePosition2Image;
    public Texture twistlungePosition3Image;
    // The angles of the player's camera are what will be read for calibration
    public Camera playerCamera;
    // 3D models of a Google Cardboard to help user orient themselves. This model will reflect the currect orientation.
    public GameObject cardboardPosition1Model;
    public GameObject cardboardPosition2Model;
    public GameObject cardboardPosition3Model;
    // Final product of this calibration is the rotations of the 2 situp positions. They're stored in this dictionary with public access.
    // The dictionary has this form: {1: Vector3, 2: Vector3}
    public Dictionary<int, Vector3> situpCalibratedRotations = new Dictionary<int, Vector3>();
    public Dictionary<int, Vector3> twistcrunchCalibratedRotations = new Dictionary<int, Vector3>();
    public Dictionary<int, Vector3> twistlungeCalibratedRotations = new Dictionary<int, Vector3>();
    // This exercise is the one that will be calibrated. Set by beginCalibration().
    private ExerciseLibrary.Exercise calibrationExercise;
    // Flag set when the user enters the calibration walkthrough. Set by beginCalibration().
    public bool userCalibrating = false;
    // Flag set to the current step in the calibration process. Reset by beginCalibration().
    private int calibrationStep = 0;
    // Reference to the Workout.cs script in order to set its variable "calibrated" to true once calibration finishes here.
    private Workout workout;

    void Start()
    {
        workout = GetComponent<Workout>();
        calibrationCanvas.enabled = false;
        cardboardPosition1Model.transform.Find("google-cardboard").transform.Find("default").GetComponent<MeshRenderer>().enabled = false;
        cardboardPosition2Model.transform.Find("google-cardboard").transform.Find("default").GetComponent<MeshRenderer>().enabled = false;
        cardboardPosition3Model.transform.Find("google-cardboard").transform.Find("default").GetComponent<MeshRenderer>().enabled = false;
        setCanvasText("");
        setCanvasImage(null);
    }

    void Update()
    {
        // Only start calibrating if user has selected it. This bool is set by an outside script from the menus.
        if(userCalibrating == true)
        {
            /* During each calibration step, the user is instructed to "Press/slide button on headset".
            *  Google Cardboard has a slider to activate a screentouch, but other alternative
            *  headsets use a button. However, they all fire a "Fire1" input in Unity.
            */
            if(Input.GetButtonDown("Fire1"))
            {
                // User has advanced to the next step in the calibration
                calibrationStep += 1;
                advanceToStep(calibrationExercise, calibrationStep);
            }
            // Show 3D models of Google Cardboard: one for each position of the situp. These will help the user see their current orientation.
            if (calibrationExercise == ExerciseLibrary.Exercise.SitUp)
            {
                if(calibrationStep==1)
                {
                    // Rotate 3D model 1 with the camera
                    cardboardPosition1Model.transform.Find("google-cardboard").transform.Find("default").GetComponent<MeshRenderer>().enabled = true;
                    cardboardPosition1Model.transform.localEulerAngles = new Vector3(
                                                    playerCamera.transform.localEulerAngles.x,
                                                    cardboardPosition1Model.transform.localEulerAngles.y,
                                                    cardboardPosition1Model.transform.localEulerAngles.z);
                }
                else if(calibrationStep==2)
                {
                    // Freeze 3D model 1 and rotate 3D model 2 with the camera
                    cardboardPosition2Model.transform.Find("google-cardboard").transform.Find("default").GetComponent<MeshRenderer>().enabled = true;
                    cardboardPosition2Model.transform.localEulerAngles = new Vector3(
                                                    playerCamera.transform.localEulerAngles.x,
                                                    cardboardPosition2Model.transform.localEulerAngles.y,
                                                    cardboardPosition2Model.transform.localEulerAngles.z);
                }
                else if(calibrationStep==0)
                {   // Remove 3D models once done or if the calibration is barely starting
                    cardboardPosition1Model.transform.Find("google-cardboard").transform.Find("default").GetComponent<MeshRenderer>().enabled = false;
                    cardboardPosition2Model.transform.Find("google-cardboard").transform.Find("default").GetComponent<MeshRenderer>().enabled = false;
                }
            }
            else if (calibrationExercise == ExerciseLibrary.Exercise.TwistCrunch)
            {
                if(calibrationStep==1)
                {
                    // Rotate 3D model 3 with the camera
                    cardboardPosition3Model.transform.Find("google-cardboard").transform.Find("default").GetComponent<MeshRenderer>().enabled = true;
                    cardboardPosition3Model.transform.localEulerAngles = new Vector3(
                                                    playerCamera.transform.localEulerAngles.x,
                                                    playerCamera.transform.localEulerAngles.y,
                                                    cardboardPosition3Model.transform.localEulerAngles.z);
                }
                else if(calibrationStep==2)
                {
                    // Freeze 3D model 3 and rotate 3D model 1 with the camera
                    cardboardPosition1Model.transform.Find("google-cardboard").transform.Find("default").GetComponent<MeshRenderer>().enabled = true;
                    cardboardPosition1Model.transform.localEulerAngles = new Vector3(
                                                    playerCamera.transform.localEulerAngles.x,
                                                    playerCamera.transform.localEulerAngles.y,
                                                    cardboardPosition1Model.transform.localEulerAngles.z);
                }
                else if(calibrationStep==3)
                {
                    // Freeze 3D model 1 and rotate 3D model 2 with the camera
                    cardboardPosition2Model.transform.Find("google-cardboard").transform.Find("default").GetComponent<MeshRenderer>().enabled = true;
                    cardboardPosition2Model.transform.localEulerAngles = new Vector3(
                                                    playerCamera.transform.localEulerAngles.x,
                                                    playerCamera.transform.localEulerAngles.y,
                                                    cardboardPosition2Model.transform.localEulerAngles.z);
                }
                else if(calibrationStep==4)
                {   // Remove 3D models once done or if the calibration is barely starting
                    cardboardPosition1Model.transform.Find("google-cardboard").transform.Find("default").GetComponent<MeshRenderer>().enabled = false;
                    cardboardPosition3Model.transform.Find("google-cardboard").transform.Find("default").GetComponent<MeshRenderer>().enabled = false;
                    cardboardPosition2Model.transform.Find("google-cardboard").transform.Find("default").GetComponent<MeshRenderer>().enabled = false;
                }
            }
            else if (calibrationExercise == ExerciseLibrary.Exercise.TwistLunge)
            {
                if(calibrationStep==1)
                {
                    // Rotate 3D model 3 with the camera
                    cardboardPosition3Model.transform.Find("google-cardboard").transform.Find("default").GetComponent<MeshRenderer>().enabled = true;
                    cardboardPosition3Model.transform.localEulerAngles = new Vector3(
                                                    cardboardPosition3Model.transform.localEulerAngles.x,
                                                    playerCamera.transform.localEulerAngles.y,
                                                    cardboardPosition3Model.transform.localEulerAngles.z);
                }
                else if(calibrationStep==2)
                {
                    // Freeze 3D model 3 and rotate 3D model 1 with the camera
                    cardboardPosition1Model.transform.Find("google-cardboard").transform.Find("default").GetComponent<MeshRenderer>().enabled = true;
                    cardboardPosition1Model.transform.localEulerAngles = new Vector3(
                                                    cardboardPosition3Model.transform.localEulerAngles.x,
                                                    playerCamera.transform.localEulerAngles.y,
                                                    cardboardPosition1Model.transform.localEulerAngles.z);
                }
                else if(calibrationStep==3)
                {
                    // Freeze 3D model 1 and rotate 3D model 2 with the camera
                    cardboardPosition2Model.transform.Find("google-cardboard").transform.Find("default").GetComponent<MeshRenderer>().enabled = true;
                    cardboardPosition2Model.transform.localEulerAngles = new Vector3(
                                                    cardboardPosition3Model.transform.localEulerAngles.x,
                                                    playerCamera.transform.localEulerAngles.y,
                                                    cardboardPosition2Model.transform.localEulerAngles.z);
                }
                else if(calibrationStep==4)
                {   // Remove 3D models once done or if the calibration is barely starting
                    cardboardPosition1Model.transform.Find("google-cardboard").transform.Find("default").GetComponent<MeshRenderer>().enabled = false;
                    cardboardPosition3Model.transform.Find("google-cardboard").transform.Find("default").GetComponent<MeshRenderer>().enabled = false;
                    cardboardPosition2Model.transform.Find("google-cardboard").transform.Find("default").GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }
    }

    /*  Function to be called by an outside script wanting to being the calibration process.
    *   Requires an exercise parameter to know which calibration sequence to start. 
     */
    public void beginCalibration(ExerciseLibrary.Exercise exerciseToCalibrate)
    {
        resetCardboardModelPositions();
        calibrationCanvas.enabled = true;
        userCalibrating = true;
        calibrationExercise = exerciseToCalibrate;
        calibrationStep = 0;

        advanceToStep(calibrationExercise, calibrationStep);
    }

    private void resetCardboardModelPositions()
    {
        cardboardPosition1Model.transform.localEulerAngles = new Vector3(0,0,0);
        cardboardPosition2Model.transform.localEulerAngles = new Vector3(0,0,0);
        cardboardPosition3Model.transform.localEulerAngles = new Vector3(0,0,0);
    }

    /*  End the calibration session and reset all necessary variables in preparation
     *  for a future calibration. The UI canvas is also removed from visibility.
     */
    private void endCalibration()
    {
        workout.calibrated = true;
        userCalibrating = false;
        calibrationStep = 0;
        calibrationCanvas.enabled = false;
        setCanvasText("");
        setCanvasImage(null);
    }

    /*  This is the main driving function of the calibration walkthrough.
     *  This function determines the UI elements and behavior of each step in
     *  the calibration depending on the chosen exercise.
     */
    private void advanceToStep(ExerciseLibrary.Exercise exerciseToCalibrate, int step)
    {
        if(calibrationExercise == ExerciseLibrary.Exercise.SitUp)
        {
            if(step == 0)
                setCanvasText("Press button on headset\nto begin sit-up calibration");
            else if(step == 1)
            {
                setCanvasText("Get in position 1 of a sit-up,\nthen press button on headset");
                setCanvasImage(situpPosition1Image);
            }
            else if(step == 2)
            {
                readPositionAngles(1);
                setCanvasText("Get in position 2 of a sit-up,\nthen press button on headset");
                setCanvasImage(situpPosition2Image);
            }
            else if(step == 3)
            {
                readPositionAngles(2);
                setCanvasText("Calibrated successfully!\nPress button on headset to begin workout");
                setCanvasImage(null);
            }
            else
                endCalibration();
        }
        else if(calibrationExercise == ExerciseLibrary.Exercise.TwistCrunch)
        {
            if(step == 0)
                setCanvasText("Press button on headset\nto begin twist-crunch calibration");
            else if(step == 1)
            {
                setCanvasText("Get in position 1 of a twist-crunch,\nthen press button on headset");
                setCanvasImage(twistcrunchPosition1Image);
            }
            else if(step == 2)
            {
                readPositionAngles(1);
                setCanvasText("Get in position 2 of a twist-crunch,\nthen press button on headset");
                setCanvasImage(twistcrunchPosition2Image);
            }
            else if(step == 3)
            {
                readPositionAngles(2);
                setCanvasText("Get in position 3 of a twist-crunch,\nthen press button on headset");
                setCanvasImage(twistcrunchPosition3Image);
            }
            else if(step == 4)
            {
                readPositionAngles(3);
                setCanvasText("Calibrated successfully!\nPress button on headset to begin workout");
                setCanvasImage(null);
            }
            else
                endCalibration();
        }
        else if(calibrationExercise == ExerciseLibrary.Exercise.TwistLunge)
        {
            if(step == 0)
                setCanvasText("Press button on headset\nto begin twist-lunge calibration");
            else if(step == 1)
            {
                setCanvasText("Get in position 1 of a twist-lunge,\nthen press button on headset");
                setCanvasImage(twistlungePosition1Image);
            }
            else if(step == 2)
            {
                readPositionAngles(1);
                setCanvasText("Get in position 2 of a twist-lunge,\nthen press button on headset");
                setCanvasImage(twistlungePosition2Image);
            }
            else if(step == 3)
            {
                readPositionAngles(2);
                setCanvasText("Get in position 3 of a twist-lunge,\nthen press button on headset");
                setCanvasImage(twistlungePosition3Image);
            }
            else if(step == 4)
            {
                readPositionAngles(3);
                setCanvasText("Calibrated successfully!\nPress button on headset to begin workout");
                setCanvasImage(null);
            }
            else
                endCalibration();
        }
        else
            setCanvasText("Calibration not supported for this exercise");
    }

    private void readPositionAngles(int pos)
    {   
        if(calibrationExercise == ExerciseLibrary.Exercise.SitUp)
            situpCalibratedRotations[pos] = new Vector3(playerCamera.transform.localEulerAngles.x, 0, 0);
        else if(calibrationExercise == ExerciseLibrary.Exercise.TwistCrunch)
        {
            if(calibrationStep == 1)
                twistcrunchCalibratedRotations[pos] = new Vector3(playerCamera.transform.localEulerAngles.x, 
                                                                    playerCamera.transform.localEulerAngles.y, 0);
            else
                twistcrunchCalibratedRotations[pos] = new Vector3(playerCamera.transform.localEulerAngles.x,
                                                                    playerCamera.transform.localEulerAngles.y, 0);
        }
        else if(calibrationExercise == ExerciseLibrary.Exercise.TwistLunge)
            twistlungeCalibratedRotations[pos] = new Vector3(0, playerCamera.transform.localEulerAngles.y, 0);
    }

    /*  Helper function to quickly change the instructional text on the
     *  calibration walkthrough.
     */
    private void setCanvasText(string message)
    {
        instructionalText.text = message;
    }

    /*  Helper function to quickly remove or change the image on the calibration walkthrough.
     *  If null is passed in, the image object is removed from visibility all together.
     */
    private void setCanvasImage(Texture tex)
    {
        if(tex == null)
            instructionalImage.enabled = false;
        else
        {
            instructionalImage.enabled = true;
            instructionalImage.texture = tex;
        }
    }
}
