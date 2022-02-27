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

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        cm = GameObject.FindObjectOfType<CluesManager>();
    }

    // Update is called once per frame
    public void MusicChange()
    {
        if (cm.CluesLeft() == 4)
            {
                audioSource.Stop();
                audioSource.loop = true;
                audioSource.PlayOneShot(music1);
            }
        else if (cm.CluesLeft() > 0 & cm.CluesLeft() < 4)
            {
                audioSource.Stop();
                audioSource.loop = true;
                audioSource.PlayOneShot(music2);
            }
        else
            {
                audioSource.Stop();
                audioSource.loop = true;
                audioSource.PlayOneShot(music3);
            }
    }
}
