using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soundtrack : MonoBehaviour
{
    public AudioClip music1;
    public AudioClip music2;
    public AudioClip music3;
    private CluesManager cm;
    private AudioSource audioSource;
    private bool played = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        cm = GameObject.FindObjectOfType<CluesManager>();
        audioSource.Stop();
        audioSource.clip = music1;
        audioSource.Play();
        audioSource.loop = true;
    }

    // Update is called once per frame
    public void MusicChange()
    {
        if ((cm.CluesLeft() > 0 & cm.CluesLeft() < 4) & (played == false))
            {
                audioSource.Stop();
                audioSource.clip = music2;
                audioSource.Play();
                audioSource.loop = true;
                played = true;
            }
        else if (cm.CluesLeft() == 0)
            {
                audioSource.Stop();
                audioSource.clip = music3;
                audioSource.Play();
                audioSource.loop = true;
            }
    }
}
