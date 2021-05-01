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

    public void triggerDemonstration()
    {
        a.SetTrigger("Demonstrate");
        a.SetBool("Busy", true);
    }
}
