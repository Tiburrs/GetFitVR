using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerOptions : MonoBehaviour
{
    private Animator a;
    public float rotSpeed;
    // Start is called before the first frame update
    void Start()
    {
        a = GetComponent<Animator>();
        a.SetTrigger("Wave");

    }

    // Update is called once per frame
    void Update()
    {
        if (a.GetBool("Busy"))
        {
            transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime);
        }
    }

    public void triggerSitup()
    {
        a.SetInteger("DemonState", 0);
        a.SetBool("Busy", true);
    }

    public void triggerLunge()
    {
        a.SetInteger("DemonState", 1);
        a.SetBool("Busy", true);
    }

    public void triggerCrunch()
    {
        a.SetInteger("DemonState", 2);
        a.SetBool("Busy", true);
    }
}
