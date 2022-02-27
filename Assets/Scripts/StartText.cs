using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartText : MonoBehaviour
{
    private GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<CameraMovement>().enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            player.GetComponent<PlayerMovement>().enabled = true;
            player.GetComponent<CameraMovement>().enabled = true;
            Destroy(gameObject);
        }
    }
}
