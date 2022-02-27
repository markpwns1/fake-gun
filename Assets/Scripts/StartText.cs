using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartText : MonoBehaviour
{
    void Start()
    {
        FindObjectOfType<PlayerMovement>().enabled = false;
        FindObjectOfType<CameraMovement>().enabled = false;
        FindObjectOfType<WeaponSway>().enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FindObjectOfType<PlayerMovement>().enabled = true;
            FindObjectOfType<CameraMovement>().enabled = true;
            FindObjectOfType<WeaponSway>().enabled = true;
            Destroy(gameObject);
        }
    }
}
