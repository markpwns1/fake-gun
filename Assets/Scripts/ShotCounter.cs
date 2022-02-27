using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShotCounter : MonoBehaviour
{
    public Gun gun;
    public GameObject warningText;
    void Start()
    {
        gun = FindObjectOfType<Gun>();
        warningText = GameObject.FindGameObjectWithTag("Warning");
    }

    void Update()
    {
        GetComponent<Text>().text = (gun.clipSize - gun.BulletShot()).ToString() + " / " + gun.clipSize;
    }
}
