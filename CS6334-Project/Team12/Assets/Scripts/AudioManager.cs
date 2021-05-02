using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioSource play;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        play = this.GetComponent<AudioSource>();
    }

    public static void ButtonClick()
    {
        play.Play();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
