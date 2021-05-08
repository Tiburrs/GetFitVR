using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerOptions : MonoBehaviour
{
    private Animator a;
    public float rotSpeed;
    public AudioClip[] clips;
    public AudioSource player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<AudioSource>();
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

    public void stop()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
        a.SetBool("Busy", false);
        player.Stop();
    }
    public void triggerSitup()
    {
        player.clip = clips[0];
        player.Play();
        a.SetInteger("DemonState", 0);
        a.SetBool("Busy", true);
    }

    public void triggerLunge()
    {
        player.clip = clips[1];
        player.Play();
        a.SetInteger("DemonState", 1);
        a.SetBool("Busy", true);
    }

    public void triggerCrunch()
    {
        player.clip = clips[2];
        player.Play();
        a.SetInteger("DemonState", 2);
        a.SetBool("Busy", true);
    }

    public void triggerHalf()
    {

        player.clip = clips[3];
        player.Play();
    }
    public void triggerComplete()
    {

        player.clip = clips[4];
        player.Play();
    }

    public void triggerRep()
    {
        player.clip = clips[5];
        player.Play();
    }
}
