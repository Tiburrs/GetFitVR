using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonstrationOptions : MonoBehaviour
{
    // Reference the to canvas containing the demonstration
    public Canvas demonstrationCanvas;
    private Canvas trainerOptionsCanvas;
    void Start()
    {
        demonstrationCanvas.enabled = false;
        trainerOptionsCanvas = GameObject.Find("Trainer Options").GetComponent<Canvas>();
    }

    void Update()
    {
        if(demonstrationCanvas.enabled == true)
        {
            // Listen for the user tapping the screen to exit the demonstrationCanvas
            if(Input.GetButtonDown("Fire1"))
            {
                demonstrationCanvas.enabled = false;
                trainerOptionsCanvas.enabled = true;
            }
        }
    }

    // Function called by the onClick event trigger of the "Demonstration" button being gazed at.
    public void CallDemonstration()
    {
        demonstrationCanvas.enabled = true;
    }
}
