using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DieScene : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void Die() {
        FindObjectOfType<PlayerMovement>().enabled = false;
        FindObjectOfType<CameraMovement>().enabled = false;
        FindObjectOfType<WeaponSway>().enabled = false;
    }
}
