using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonstrationOptions : MonoBehaviour
{
    // Reference the to canvas containing the demonstration
    public Canvas demonstrationCanvas;

    void Start()
    {
        demonstrationCanvas.enabled = false;
    }

    void Update()
    {
        if(demonstrationCanvas.enabled == true)
        {
            // Listen for the user tapping the screen to exit the demonstrationCanvas
            if(Input.GetButtonDown("Fire1"))
                demonstrationCanvas.enabled = false;
        }
    }

    // Function called by the onClick event trigger of the "Demonstration" button being gazed at.
    public void CallDemonstration()
    {
        demonstrationCanvas.enabled = true;
    }
}
