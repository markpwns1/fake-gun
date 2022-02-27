using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShotCounter : MonoBehaviour
{
    public Gun gun;
    public GameObject warningText;

    IEnumerator FlashingCoroutine()
    {
        while (true)
        {
            warningText.GetComponent<Text>().color = Color.white;
            yield return new WaitForSeconds(0.5f);
            warningText.GetComponent<Text>().color = Color.red;
            yield return new WaitForSeconds(0.5f);
        }
    }

    void Start()
    {
        gun = FindObjectOfType<Gun>();
        warningText = GameObject.FindGameObjectWithTag("Warning");
        warningText.SetActive(false);
    }

    void Update()
    {
        GetComponent<Text>().text = (gun.clipSize - gun.BulletShot()).ToString() + " / " + gun.clipSize;
        if (gun.BulletShot() >= gun.clipSize)
        {
            warningText.SetActive(true);
            var c = StartCoroutine(FlashingCoroutine());
        }
        else {
            warningText.SetActive(false);
        }
    }
}
