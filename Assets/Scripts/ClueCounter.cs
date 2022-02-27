using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClueCounter : MonoBehaviour
{
    private CluesManager cm;
    private void Start()
    {
        cm = FindObjectOfType<CluesManager>();
    }
    void Update()
    {
        if (cm.CluesLeft() > 0)
        {
            GetComponent<Text>().text = (4 - cm.CluesLeft()) + " / 4 documents collected.\nLook for brown tables!";
        }
        else {
            GetComponent<Text>().text = "All documents collected. Escape!";
        }
    }
}
