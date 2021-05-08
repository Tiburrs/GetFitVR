using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GazePointerSelect : MonoBehaviour
{
    public Image theImage;
    public UnityEvent GVRClick;
    public float totalTime = 2;
    
    bool gvrStatus;
    public float gvrTimer;

    // Update is called once per frame
    void Update()
    {
        if(gvrStatus)
        {
            gvrTimer += Time.deltaTime;
            theImage.fillAmount = gvrTimer / totalTime;

            if(theImage.fillAmount == 1)
            {
                theImage.fillAmount = 0;
            }
        }
        else
        {
        gvrTimer = 0;
        }

        if(gvrTimer > totalTime&&gvrStatus)
        {
            gvrStatus = false;
            AudioManager.ButtonClick();
            GVRClick.Invoke();
        }
    }

    public void GVROn()
    {
        
        gvrStatus = true;
    }

    public void GVROff()
    {
        gvrStatus = false;
      
        theImage.fillAmount = 0;
    }
}
