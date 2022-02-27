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
            GetComponent<Text>().text = cm.CluesLeft().ToString() + " clues left";
        }
        else {
            GetComponent<Text>().text = "Clue collection completed. Escape!";
        }
    }
}
