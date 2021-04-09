/* This script has no behavior. Its purpose is to store the various exercises
 * supported by this application. This allows other scripts to universally 
 * reference this script when they want to reference an exercise.
 * All they have to do is reference ExerciseLibrary.Exercise.<name>
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExerciseLibrary : MonoBehaviour
{
    // Enum used to refer to the various exercises supported by this app
    public enum Exercise
    {
        SitUp,
        TwistCrunch,
        TwistLunge
    }
}
