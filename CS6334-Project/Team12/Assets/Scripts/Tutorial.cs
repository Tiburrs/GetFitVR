/* This is the script in charge of walking the user through the tutorial
 * available via the Menu Scene (the welcome scene).
 * 
 * Once the user gazes long enough at the "Tutorial" button,
 * this script fires and shows a canvas with instructions on how to use
 * this app.
 *
 * This script is attached to the Character asset for consistency with the
 * similar Calibration and Workout scripts in the workout scene.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    // Reference the to canvas containing the tutorial
    public Canvas tutorialCanvas;
    public Canvas menu;

    void Start()
    {
        tutorialCanvas.enabled = false;
    }

    void Update()
    {
        if(tutorialCanvas.enabled == true)
        {
            // Listen for the user tapping the screen to exit the tutorial
            if (Input.GetButtonDown("Fire1"))
            {
                tutorialCanvas.enabled = false;
                menu.enabled = true;
            }
        }
    }

    // Function called by the onClick event trigger of the "Tutorial" button being gazed at.
    public void CallTutorial()
    {
        tutorialCanvas.enabled = true;
    }
}
