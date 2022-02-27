using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShotCounter : MonoBehaviour
{
    private Gun gun;
    private GameObject warningText;

    IEnumerator FlashingCoroutine()
    {
        while (true)
        {
            warningText.GetComponent<Text>().color = new Color(0, 0, 0, 0);
            yield return new WaitForSeconds(0.5f);
            warningText.GetComponent<Text>().color = new Color(0, 0, 0, 1);
            yield return new WaitForSeconds(0.5f);
        }
    }

    void Start()
    {
        gun = FindObjectOfType<Gun>();
        warningText = GameObject.FindGameObjectWithTag("Warning");
        warningText.SetActive(true);
        var c = StartCoroutine(FlashingCoroutine());
        warningText.SetActive(false);
    }

    void Update()
    {
        GetComponent<Text>().text = (gun.clipSize - gun.BulletShot()).ToString() + " / " + gun.clipSize;
        if (gun.BulletShot() >= gun.clipSize)
        {
            warningText.SetActive(true);
        }
        else {
            warningText.SetActive(false);
        }
    }
}
